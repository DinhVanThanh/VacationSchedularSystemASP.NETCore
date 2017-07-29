using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.common.core.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Adapter;
using ProfileService.IServiceInterfaces;
using ProfileService.Model;
using ProfileService.Model.BindingModel;
using ProfileService.Model.ViewModel;
using App.common.core.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ProfileService.Controllers
{
    
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        private string AVATAR_FOLDER = "Avatars";
        private string AVATAR_FOLDER_FULL_PATH; 

        private ITeamService _teamService;
        private IEmployeeService _employeeService;
        private IHostingEnvironment _env;

        public EmployeeController(ITeamService teamService, IEmployeeService employeeService,  IHostingEnvironment env)
        {
            _env = env;
            _teamService = teamService;
            _employeeService = employeeService;
            AVATAR_FOLDER_FULL_PATH = Path.Combine(env.WebRootPath, AVATAR_FOLDER);
           
        }
        [Authorize]
        [HttpGet]
        [Route("me")]
        public EmployeeViewModel MyProfile()
        {
            int? id = User.GetAccountId();
            Employee existingEmployee = _employeeService.Get(id.HasValue ? id.Value : -1);
            return EmployeeAdapter.ToViewModel(existingEmployee);
        }
        // GET: api/Employee
        [HttpGet]
        [Route("EmployeeList")]
        public  IEnumerable<EmployeeViewModel> Get()
        {
            var EmployeeViewModelList = new List<EmployeeViewModel>();
            foreach (Employee employee in  _employeeService.GetAll())
            {
                EmployeeViewModelList.Add(EmployeeAdapter.ToViewModel(employee));
            }
            return EmployeeViewModelList;
            
        }

        // GET: api/Employee/5
        [HttpGet]
        [Route("GetScheduleOwners")]
        public IEnumerable<ScheduleOwnerViewModel> GetScheduleOwners()
        {
            List<ScheduleOwnerViewModel> ScheduleViewModels = (List<ScheduleOwnerViewModel>) EmployeeAdapter.ToScheduleViewModel(_employeeService.GetAll());
             
            ScheduleViewModels.Add(new ScheduleOwnerViewModel { Id = - 1, Name = "Select All"});

            return ScheduleViewModels;
        }

        // POST: api/Employee/UpdateInfo
        [HttpPut]
        [Route("UpdateInfo")]
        public IActionResult Update([FromBody]EmployeeBindingModel EmployeeBindingModel)
        {
            int EmployeeIsUpdatedSuccessfully = _employeeService.Update(EmployeeAdapter.ToModel(EmployeeBindingModel));
            if (EmployeeIsUpdatedSuccessfully == 1)
                return Ok(EmployeeBindingModel);
            else
                return BadRequest();
        }

        // POST: api/Employee/Create
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody]EmployeeBindingModel EmployeeBindingModel)
        {
             int EmployeeIsCreatedSuccessfully = _employeeService.Create(EmployeeAdapter.ToModel(EmployeeBindingModel));
            if (EmployeeIsCreatedSuccessfully == 1)
                return Ok(EmployeeBindingModel);
            else
                return NoContent(); 
        }
        
        // PUT: api/Employee/5
        [HttpDelete]
        [Route("Delete/{EmployeeId}")]
        public IActionResult Delete(int EmployeeId)
        {
            int EmployeeIsRemovedSuccessfully = _employeeService.Remove(EmployeeId);
            if (EmployeeIsRemovedSuccessfully == 1)
                return Ok();
            else
                return NoContent();
        }
         
        // POST: api/Employee/UploadAvatar
        [HttpPost]
        [Route("UploadAvatar/{EmployeeId}")]
        public IActionResult UploadAvatar(int EmployeeId)
        {
            string[] validExts = new string[] { ".jpeg", ".jpg", ".png" };
            var files = Request.Form.Files;
            if (files == null || files.Count != 1)
            {
                throw new NullReferenceException();
            }
            var file = files.FirstOrDefault();
            var filename = Uploader.GetFileName(file);
            var fileExtension = Path.GetExtension(filename).ToLower();

            if (validExts.Contains(fileExtension))
            {
                var newFileName = Guid.NewGuid().ToString("N") + fileExtension;
                Uploader.Upload(file, AVATAR_FOLDER_FULL_PATH, newFileName);

                var url = $"/{AVATAR_FOLDER}/{newFileName}";

                try
                {
                     _employeeService.UpdateAvatar(EmployeeId, url);
                    return Ok() ;
                }
                catch (Exception e)
                {
                    System.IO.File.Delete(Path.Combine(AVATAR_FOLDER_FULL_PATH, newFileName));
                    throw e;
                }
            }
            else
            {
                throw new FileLoadException("Invalid File Extension");
                 
            }
        }
    }
}
