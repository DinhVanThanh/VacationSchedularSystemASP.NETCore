using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.Model;

namespace ProfileService.IServiceInterfaces
{
    public interface ITeamService
    {
        Team Get(int TeamId);
        List<Team> GetAllTeam();
        int Update(Team team);
        int Create(Team team);
    }
}
