using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.Model;
using ProfileService.Model.BindingModel;
using ProfileService.Model.ViewModel;

namespace ProfileService.Adapter
{
    public class EmployeeAdapter
    {
        public static EmployeeViewModel ToViewModel(Employee employee)
        {
            var ViewModel = new EmployeeViewModel();
            ViewModel.Id = employee.EmployeeId;
            ViewModel.FirstName = employee.FirstName;
            ViewModel.LastName = employee.LastName;
            ViewModel.PhoneNumber = employee.PhoneNumber;
            ViewModel.RemainDayOff = (int)employee.RemainingDay;
            ViewModel.Email = employee.Email;
            ViewModel.PositionId = (int)employee.PositionId;
            ViewModel.Avatar = employee.Avatar;
            ViewModel.BirthDate = (DateTime)employee.BirthDate;
            return ViewModel;
        }
        public static Employee ToModel(EmployeeBindingModel employee)
        {
            var ViewModel = new Employee();
            ViewModel.EmployeeId = employee.Id;
            ViewModel.FirstName = employee.FirstName;
            ViewModel.LastName = employee.LastName;
            ViewModel.PhoneNumber = employee.PhoneNumber;
            ViewModel.RemainingDay = employee.RemainDayOff;
            ViewModel.Email = employee.Email;
            ViewModel.PositionId = employee.PositionId;
            ViewModel.Avatar = employee.Avatar;
            ViewModel.BirthDate = employee.BirthDate;
            return ViewModel;
        }

        public static IEnumerable<ScheduleOwnerViewModel> ToScheduleViewModel(IEnumerable<Employee> employees)
        {
            IList<ScheduleOwnerViewModel> ScheduleOwnerViewModelList = new List<ScheduleOwnerViewModel>();
            foreach (Employee employee in employees)
            {
                ScheduleOwnerViewModelList.Add(new ScheduleOwnerViewModel() { Id = employee.EmployeeId, Name = employee.LastName + " " + employee.FirstName });
            }
            return ScheduleOwnerViewModelList;
        }
    }
}
