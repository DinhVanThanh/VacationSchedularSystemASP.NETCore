using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ProfileService.IServiceInterfaces;
using ProfileService.Model;

namespace ProfileService.Services
{
    public class EmployeeService : IEmployeeService
    {

        private IHostingEnvironment _env;
        private ProfileContext _profileContext;
        public EmployeeService(ProfileContext profileContext, IHostingEnvironment env)
        {
            _env = env;
            _profileContext = profileContext;
        }
        public int Create(Employee employee)
        {
            employee.RemainingDay = 12;
            _profileContext.Add(employee);
            return _profileContext.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        {
            return  _profileContext.Employees.ToList();
        }

        public Employee Get(int Id)
        {
            return _profileContext.Employees.SingleOrDefault(s => s.EmployeeId == Id);
        }

        public int Remove(int EmployeeId)
        {
            _profileContext.Remove(Get(EmployeeId));
            return _profileContext.SaveChanges();
        }

        public Task<int> RemovePositionAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public int Update(Employee employee)
        {
            _profileContext.Update(employee);
            return _profileContext.SaveChanges();
        }

        public async Task UpdateAvatar(int EmployeeId, string Url)
        {
            var existingEmployee = Get(EmployeeId);
            if (existingEmployee == null)
                throw new NullReferenceException("Null Employee");
            if (!string.IsNullOrEmpty(existingEmployee.Avatar))
            {
                try
                {
                    string fullPath = Path.Combine(_env.WebRootPath, existingEmployee.Avatar.Trim('/'));
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                }
                catch (Exception)
                {
                }
            }

            existingEmployee.Avatar = Url;
            await _profileContext.SaveChangesAsync();
        }
    }

     
}
