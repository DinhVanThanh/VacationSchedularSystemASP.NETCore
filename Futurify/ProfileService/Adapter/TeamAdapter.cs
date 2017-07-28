using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.IServiceInterfaces;
using ProfileService.Model;
using ProfileService.Model.BindingModel;
using ProfileService.Model.ViewModel;

namespace ProfileService.Adapter
{
    public class TeamAdapter
    {
        public static IEmployeeService _employeeService { get; set; }


        public static IEnumerable<EmployeeTeamViewModel> ToEmployeeViewModel(IEnumerable<Employee> employees)
        {
            List<EmployeeTeamViewModel> employeeTeamViewModels = new List<EmployeeTeamViewModel>();
            foreach (Employee employee in employees)
            {
                EmployeeTeamViewModel employeeViewModel = new EmployeeTeamViewModel();
                employeeViewModel.Id = employee.EmployeeId;
                employeeViewModel.Name = employee.FirstName + " " + employee.LastName;
                employeeTeamViewModels.Add(employeeViewModel);
            }

            return employeeTeamViewModels;
        }

        public static Team ToTeamModel(TeamBindingModel teamBindingModel)
        {
            List<Employee> teamMembers = new List<Employee>();
            foreach (int MemberId in teamBindingModel.MemberIds)
            {
                teamMembers.Add(_employeeService.Get(MemberId));
            }
            Team team = new Team();
            team.TeamId = teamBindingModel.Id;
            team.LeaderId = teamBindingModel.LeaderId;
            team.Leader = _employeeService.Get(teamBindingModel.LeaderId);
            team.TeamName = teamBindingModel.TeamName;
            team.Employees = teamMembers;
            return team;
        }

        public static IEnumerable<TeamViewModel> ToTeamViewModel(IEnumerable<Team> Teams)
        {
           
            IList<TeamViewModel> TeamViewModelList = new List<TeamViewModel>();
            foreach(Team team in Teams)
            {
                IList<int> teamMemberIds = new List<int>();
                foreach (Employee member in team.Employees)
                {
                    teamMemberIds.Add(member.EmployeeId);
                }
                TeamViewModel teamViewModel = new TeamViewModel();
                teamViewModel.Id = team.TeamId;
                teamViewModel.Name = team.TeamName;
                teamViewModel.LeaderId = team.LeaderId;
                teamViewModel.MemberIds = teamMemberIds;
                TeamViewModelList.Add(teamViewModel);
            }
            return TeamViewModelList;
        }
    }
}
