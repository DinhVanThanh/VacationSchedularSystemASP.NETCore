using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.common.core;
using App.common.core.Exceptions;
using App.common.core.Messaging;
using Microsoft.EntityFrameworkCore;
using ProfileService.Model;
using RawRabbit.Context;
using Vacation.common.Constant; 
using Vacation.common.Events;

namespace ProfileService.EventHandlers
{
    public class AccountCreatedHandlers : IMessageHandle<AccountCreatedForEmail>
    {
        private DbContextOptions<ProfileContext> _accountContext;

        public AccountCreatedHandlers(DbContextOptions<ProfileContext> accountContext)
        {
            _accountContext = accountContext;
        }

        public async Task HandleAsync(AccountCreatedForEmail e, IMessageContext context)
        {
            try
            {
                var db = new ProfileContext(_accountContext);
                Employee existEmployee = await db.Employees.Where(x => x.Email == e.Email).FirstOrDefaultAsync();
                if (existEmployee == null)
                {
                    Employee newEmployee = new Employee(); 
                    newEmployee.Position = db.Positions.FirstOrDefault(p => p.PositionId == e.Position);
                    newEmployee.Email = e.Email;
                    newEmployee.BirthDate = e.Birthday;
                    newEmployee.PhoneNumber = e.PhoneNumber;
                    newEmployee.FirstName = e.FirstName;
                    newEmployee.LastName = e.LastName;
                    newEmployee.Gender = e.Gender;
                    newEmployee.RemainingDay = CommonConstant.RemaimDayOff;
                    db.Employees.Add(newEmployee);
                    await db.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw new CustomException("CREATE EMPLOYEE ERROR");
            }
            
        }
    }
}
