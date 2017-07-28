using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.Model;

namespace ProfileService.IServiceInterfaces
{
    public interface IEmployeeService
    {
        Employee Get(int Id);
        IEnumerable<Employee> GetAll();
        int Create(Employee employee);
        int Update(Employee employee);
        Task<int> RemovePositionAsync(int employeeId);
        Task UpdateAvatar(int EmployeeId, string Url);
        int Remove(int EmployeeId);
    }
}
