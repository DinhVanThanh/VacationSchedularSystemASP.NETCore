using App.Common.Core.Authentication;
using App.Common.Core.Exceptions;
using JobHop.Common.Models;
using AuthenticationServer.Adapters;
using AuthenticationServer.Models;
using AuthenticationServer.Models.BindingModels;
using AuthenticationServer.Models.ViewModels;
using AuthenticationServer.Providers;
using AuthenticationServer.Resources;
using AuthenticationServer.ServicesInterfaces;
using AuthenticationServer.Setup;
using JobHop.Common.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PhoneNumbers;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using JobHop.Common.Enums;
using AuthenticationServer.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationServer.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {

        IAccountService _accountService;
        private readonly TokenProviderOptions _tokenOptions;
        private readonly TwilioProviderOptions _twilioOptions;
        private readonly FacebookProviderOptions _facebookOptions;
        private readonly EmailTemplate _emailTemplate;
        private readonly IESMSService _esmsService;
        private IHostingEnvironment _env;
        private IBusClient _rawRabbitBus;


        public AccountController(IAccountService accountSrv, IOptions<TokenProviderOptions> tokenOptions, IHostingEnvironment env, 
            IBusClient rawRabbitBus, IOptions<TwilioProviderOptions> twilioOptions, IOptions<FacebookProviderOptions> facebookOptions,
            IESMSService esmsService, IOptions<EmailTemplate> emailTemplate)
        {
            _accountService = accountSrv;
            _env = env;
            _tokenOptions = tokenOptions.Value;
            _rawRabbitBus = rawRabbitBus;
            _twilioOptions = twilioOptions.Value;
            _facebookOptions = facebookOptions.Value;
            _esmsService = esmsService;
            _emailTemplate = emailTemplate.Value;
        }

        [Authorize(Roles = "VIEW_ACCOUNTS")]
        [HttpGet, Route("{id:int}")]
        public async Task<AccountViewModel> Get(int id)
        {
            var account = await _accountService.FindByIdAsync(id);

            return account.ToViewModel();
        }

        [Authorize]
        [HttpGet, Route("me")]
        public async Task<AccountViewModel> MyAccount()
        {
            var id = User.GetAccountId();

            var account = await _accountService.FindByIdAsync(id.Value);

            return account.ToViewModel();
        }

        [Authorize]
        [HttpPut, Route("me/password")]
        public async Task ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_REQUEST, Errors.INVALID_REQUEST_MSG);
            }

            await _accountService.ChangePasswordAsync(User.GetAccountId().Value, model.OldPassword, model.NewPassword);
        }

        [Authorize]
        [HttpGet("me/permissions")]
        public IEnumerable<string> MyPermissions()
        {
            return User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        // GET api/account/is-me-authenticated
        [HttpGet("is-me-authenticated")]
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        [HttpGet("is-user-name-available")]
        public async Task<bool> IsUserNameAvailable([FromQuery]string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            if (!userName.Contains("@"))
            {
                try
                {
                    var formatedPhoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(userName);
                    userName = formatedPhoneNumber;
                }
                catch (Exception)
                {
                }
            }

            return (await _accountService.FindByUserNameAsync(userName)) == null;
        }

        [HttpGet("is-email-available")]
        public async Task<bool> IsEmailAvailable([FromQuery]string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            return (await _accountService.FindByEmailAsync(email, null)) == null;
        }

        [HttpGet("facebook-login/exist")]
        public async Task<bool> FacebookerHasLocalAccount([FromQuery]string facebookId)
        {
            if (string.IsNullOrEmpty(facebookId))
            {
                return false;
            }
            var exist = await _accountService.FindByExternalProviderAsync("facebook", facebookId);
            return exist != null;
        }

        /// <summary>
        /// login by facebook, only use for jobseeker
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("facebook-login")]
        public async Task<SignInResponseModel> FacebookLogin([FromBody]FacebookLoginModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_REQUEST, Errors.INVALID_REQUEST_MSG);
            }

            var verifyResult = await this.VerifyFacebookAccessToken(model.AccessToken);

            if (verifyResult.IsValid)
            {
                var existAccount = await _accountService.FindByExternalProviderAsync("facebook", verifyResult.FacebookId);

                if (existAccount == null)
                {
                    throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
                }
                else if(existAccount.Status == UserStatus.InActive)
                {
                    throw new CustomException(Errors.ACCOUNT_INACTIVE, Errors.ACCOUNT_INACTIVE_MSG);
                }

                if (existAccount.AccountType == JobHop.Common.Enums.AccountType.Jobseeker && !existAccount.PhoneNumberVerified)
                {
                    //return phone number in message for verify
                    throw new CustomException(Errors.WAIT_FOR_VERIFICATION, existAccount.PhoneNumber);
                }

                var uname = string.IsNullOrEmpty(existAccount.UserName) ? $"Facebook:{verifyResult.FacebookId}" : existAccount.UserName;

                var permissions = await _accountService.GetPermissionsOfAccountAsync(existAccount.Id);

                var now = DateTime.UtcNow;

                var encodedJwt = TokenProviderMiddleware.GenerateAccessToken(_tokenOptions, now, uname, existAccount.Id.ToString(), permissions.ToArray());

                var response = new SignInResponseModel
                {
                    AccessToken = encodedJwt,
                    Expires = now.AddSeconds((int)_tokenOptions.Expiration.TotalSeconds),
                    Account = existAccount.ToViewModel()
                };

                return response;
            }
            else
            {
                throw new CustomException(Errors.INVALID_REQUEST, Errors.INVALID_REQUEST_MSG);
            }
        }

        /// <summary>
        /// register by facebook, only use for jobseeker
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("facebook-register")]
        public async Task<AccountViewModel> FacebookRegister([FromBody]FacebookRegisterModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_REQUEST, Errors.INVALID_REQUEST_MSG);
            }

            try
            {
                //standardize phone number
                var formatedPhoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(model.PhoneNumber);
                model.PhoneNumber = formatedPhoneNumber;
            }
            catch (NumberParseException)
            {
                throw new CustomException(Errors.INVALID_PHONE_NUMBER, Errors.INVALID_PHONE_NUMBER_MSG);
            }

            var verifyResult = await this.VerifyFacebookAccessToken(model.AccessToken);

            if (verifyResult.IsValid)
            {
                var existAccount = await _accountService.FindByExternalProviderAsync("facebook", verifyResult.FacebookId);

                if (existAccount != null)
                {
                    throw new CustomException(Errors.EXTERNAL_LOGIN_EXIST, Errors.EXTERNAL_LOGIN_EXIST_MSG);
                }

                var account = new Account
                {
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    AccountType = JobHop.Common.Enums.AccountType.Jobseeker,
                    ExternalLogins = new List<ExternalProvider>()
                    {
                        new ExternalProvider
                        {
                            Provider = "Facebook",
                            ProviderKey = verifyResult.FacebookId
                        }
                    },
                    AccountPermissions = new List<Models.AccountPermission>()
                    {
                         new Models.AccountPermission
                         {
                              PermissionId = PermissionsList.JOBSEEKER_PERMISSION
                         }
                    }
                };

                account = await _accountService.CreateAsync(account, null, true);


                //publish a jobseeker account is created message to rabbit mq bus
                await _rawRabbitBus.PublishAsync(new JobseekerAccountCreated
                {
                    Id = account.Id,
                    PhoneNumber = account.PhoneNumber,
                    Avatar = model.Avatar,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Status = account.Status
                });

                string smsContent = $"Verification code at JobHop: {account.VerificationCodes.First().VerifyCode}";

                //send SMS using Twillio
                //var message = await MessageResource.CreateAsync(to: new Twilio.Types.PhoneNumber(account.PhoneNumber), from: new Twilio.Types.PhoneNumber(_twilioOptions.FromNumber), body: smsContent);

                //send SMS using eSMS.vn
                var response = await _esmsService.SendSMS(account.PhoneNumber, smsContent, 4);

                var viewModel = account.ToViewModel();

                return viewModel;
            }
            else
            {
                throw new CustomException(Errors.INVALID_REQUEST, Errors.INVALID_REQUEST_MSG);
            }
        }

        /// <summary>
        /// Verify facebook user access token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private async Task<FacebookVerifiedResponse> VerifyFacebookAccessToken(string accessToken)
        {
            HttpClient http = new HttpClient();

            string requestUrl = $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={_facebookOptions.AppToken}";
            var response = await http.GetAsync(requestUrl);

            var result = new FacebookVerifiedResponse();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var debugResponse = JsonConvert.DeserializeObject<FacebookDebugTokenResponse>(content);

                if (debugResponse != null && debugResponse.Data != null && _facebookOptions.AppId == debugResponse.Data.AppId)
                {
                    result.IsValid = true;
                    result.FacebookId = debugResponse.Data.FacebookId;
                }
            }

            return result;
        }

        /// <summary>
        /// Register account api for jobseeker
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("register")]
        public async Task<AccountViewModel> Register([FromBody]RegisterModel model)
        {
            var account = model.RegisterModelToModel();
            if (account == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_REGISTRATION_DATA, Errors.INVALID_REGISTRATION_DATA_MSG);
            }

            try
            {
                //standardize phone number
                var formatedPhoneNumber = PhoneNumberHelpers.GetFormatedPhoneNumber(model.PhoneNumber);
                account.UserName = formatedPhoneNumber;
                account.PhoneNumber = formatedPhoneNumber;
            }
            catch (NumberParseException)
            {
                throw new CustomException(Errors.INVALID_PHONE_NUMBER, Errors.INVALID_PHONE_NUMBER_MSG);
            }

            account.AccountPermissions = new List<Models.AccountPermission>()
            {
                 new Models.AccountPermission
                 {
                      PermissionId = PermissionsList.JOBSEEKER_PERMISSION
                 }
            };

            account = await _accountService.CreateAsync(account, model.Password);

            //publish a jobseeker account is created message to rabbit mq bus
            await _rawRabbitBus.PublishAsync(new JobseekerAccountCreated { Id = account.Id, PhoneNumber = account.PhoneNumber, Status = account.Status, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email });

            string smsContent = $"Verification code at JobHop: {account.VerificationCodes.First().VerifyCode}";

            //send SMS using eSMS.vn
            var response = await _esmsService.SendSMS(account.PhoneNumber, smsContent, 4);

            var viewModel = account.ToViewModel();

            return viewModel;
        }

        [Authorize(Roles = "CREATE_ACCOUNT")]
        [HttpPost, Route("recruiter")]
        public async Task<AccountViewModel> CreateRecruiter([FromBody]CreateRecruiterModel bindingModel)
        {
            var account = bindingModel.CreateRecruiterModelToAccountModel();

            if (account == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_REGISTRATION_DATA, Errors.INVALID_REGISTRATION_DATA_MSG);
            }

            account.AccountPermissions = new List<Models.AccountPermission>()
            {
                 new Models.AccountPermission
                 {
                      PermissionId = PermissionsList.RECRUITER_PERMISSION
                 }
            };

            account = await _accountService.CreateAsync(account, bindingModel.Password);

            try
            {
                //publish a recruiter account is created message to rabbit mq bus and wait for response
                var response = await _rawRabbitBus.RequestAsync<RecruiterAccountCreated, RpcResult>(new RecruiterAccountCreated
                {
                    Id = account.Id,
                    Email = account.Email,
                    PhoneNumber = account.PhoneNumber,
                    FirstName = bindingModel.FirstName,
                    LastName = bindingModel.LastName,
                    Gender = bindingModel.Gender,
                    Status = account.Status
                });
                if (!response.Success)
                {
                    // account created but profile creating has an error, should be log and check
                    if (!string.IsNullOrEmpty(response.Error))
                    {
                        throw new CustomException(response.Error);
                    }
                }
            }
            catch (System.Exception)
            {
                //got unhandle exception while creating recruiter profile, should be logs and correct consistency between 2 server
                throw;
            }

            var viewModel = account.ToViewModel();
            return viewModel;
        }

        [Authorize(Roles = "DELETE_ACCOUNT")]
        [HttpDelete, Route("recruiter/{id}")]
        public async Task DeleteRecruiter(int id)
        {
            bool isAccountNotFound = false;
            bool isException = false;
            try
            {
                await _accountService.DeleteRecruiter(id);
            }
            catch (CustomException ex)
            {
                isException = true;
                if (ex.Code == Errors.ACCOUNT_NOT_FOUND)
                {
                    isAccountNotFound = true;
                }
                else
                {
                    throw ex;
                }
            }
            catch (System.Exception)
            {
                isException = true;
                throw;
            }
            finally
            {
                if (!isException || isAccountNotFound)
                {
                    try
                    {
                        var response = await _rawRabbitBus.RequestAsync<RecruiterAccountDeleted, RpcResult>(new RecruiterAccountDeleted { Id = id });
                        if (!response.Success)
                        {
                            // account deleted but profile deleting has an error, should be log and check
                            if (!string.IsNullOrEmpty(response.Error))
                            {
                                throw new CustomException(response.Error);
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        //got unhandle exception while deleting recruiter profile, should be logs and correct consistency between 2 server
                        throw;
                    }
                }
            }

        }

        [Authorize(Roles = "VIEW_ACCOUNTS")]
        [HttpGet, Route("{accountId:int}/roles")]
        public async Task<IEnumerable<RoleViewModel>> GetRolesOfUser(int accountId)
        {
            var roles = await _accountService.GetRolesOfAccountAsync(accountId);

            return roles.ToListRoleViewModels();
        }

        [Authorize(Roles = "EDIT_ACCOUNT")]
        [HttpPut, Route("{accountId:int}/set-role")]
        public async Task SetRoleForUser(int accountId, [FromBody]SetRemoveRoleModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_ACCOUNT_ROLE_COMMAND_DATA, Errors.INVALID_ACCOUNT_ROLE_COMMAND_DATA_MSG);
            }

            await _accountService.SetRoleAsync(accountId, model.RoleId);
        }

        [Authorize(Roles = "EDIT_ACCOUNT")]
        [HttpPut, Route("{accountId:int}/remove-role")]
        public async Task RemoveRoleOfUser(int accountId, [FromBody]SetRemoveRoleModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_ACCOUNT_ROLE_COMMAND_DATA, Errors.INVALID_ACCOUNT_ROLE_COMMAND_DATA_MSG);
            }

            await _accountService.RemoveRoleAsync(accountId, model.RoleId);
        }

        [Authorize(Roles = "EDIT_ACCOUNT")]
        [HttpPut, Route("{accountId:int}/set-status")]
        public async Task SetStatusForUser(int accountId, [FromBody]SetStatusModel model)
        {
            await _accountService.SetStatusAsync(accountId, model.Status);
            var eventModel = new StatusProfileChanged { Status = model.Status };
            if (model.Type == AccountType.Jobseeker)
            {
                eventModel.JobseekerId = accountId;
            }
            else if (model.Type == AccountType.Recruiter)
            {
                eventModel.RecruiterId = accountId;
            }
            await _rawRabbitBus.PublishAsync(eventModel);
        }

        [HttpGet, Route("reset-password/email-request")]
        public async Task RequestResetPasswordByEmail([FromQuery]string email)
        {
            var account = await _accountService.CheckExsitByUserNameAsync(email);
            if (account == null)
            {
                throw new CustomException(Errors.ACCOUNT_NOT_FOUND, Errors.ACCOUNT_NOT_FOUND_MSG);
            }

            var request = await _accountService.CreateRequestResetPassword(new RequestResetPassword { Email = email, ExpiredAt = DateTime.Now.AddDays(1) });

            await _rawRabbitBus.PublishAsync(new PushEmail { Title = _emailTemplate.Title, Body = string.Format(_emailTemplate.Body, account.UserName, request.Token), SendTo = email });
        }

        /// <summary>
        /// Reset password by phone number
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("reset-password")]
        public async Task ResetPasswordByPhoneNumber([FromBody] PhoneNumberResetPassword model)
        {
            if (model == null)
            {
                throw new CustomException(Errors.REQUEST_NOT_NULL, Errors.REQUEST_NOT_NULL_MSG);
            }
            else if (string.IsNullOrEmpty(model.PhoneNumber))
            {
                throw new CustomException(Errors.INVALID_PHONE_NUMBER, Errors.INVALID_PHONE_NUMBER_MSG);
            }
            else if (string.IsNullOrEmpty(model.PIN))
            {
                throw new CustomException(Errors.PIN_NOT_NULL, Errors.PIN_NOT_NULL_MSG);
            }
            else if(string.IsNullOrEmpty(model.NewPassword))
            {
                throw new CustomException(Errors.PASSWORD_NOT_NULL, Errors.PASSWORD_NOT_NULL_MSG);
            }

            await _accountService.ResetPasswordByPhoneNumber(model);
        }

        /// <summary>
        /// Reset password by email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("reset-password/email")]
        public async Task ResetPasswordByEmail([FromBody] EmailResetPassword model)
        {
            if (model == null)
            {
                throw new CustomException(Errors.REQUEST_NOT_NULL, Errors.REQUEST_NOT_NULL_MSG);
            }
            else if (string.IsNullOrEmpty(model.Email))
            {
                throw new CustomException(Errors.EMAIL_NOT_NULL, Errors.EMAIL_NOT_NULL_MSG);
            }
            else if (string.IsNullOrEmpty(model.Token))
            {
                throw new CustomException(Errors.TOKEN_NOT_NULL, Errors.TOKEN_NOT_NULL_MSG);
            }
            else if (string.IsNullOrEmpty(model.NewPassword))
            {
                throw new CustomException(Errors.PASSWORD_NOT_NULL, Errors.PASSWORD_NOT_NULL_MSG);
            }

            await _accountService.ResetPasswordByEmail(model);
        }
    }
}
