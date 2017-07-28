using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.IServiceInterfaces;
using ProfileService.Model;
using Microsoft.EntityFrameworkCore;

namespace ProfileService.Services
{
    public class TeamSevice : ITeamService
    {
        private ProfileContext _context;
        public TeamSevice(ProfileContext context)
        {
            _context = context;
        }

        public int Create(Team team)
        {
            _context.Teams.Add(team);
            return _context.SaveChanges();
        }

        public Team Get(int TeamId)
        {
            return _context.Teams.Include(e => e.Employees).SingleOrDefault(s => s.TeamId == TeamId);
        }

        public List<Team> GetAllTeam()
        { 
            return _context.Teams.Include(e => e.Employees).ToList();
        }

        public int Update(Team team)
        {
            Team existingTeam = _context.Teams.Include(e => e.Employees).SingleOrDefault(t => t.TeamId == team.TeamId);
            existingTeam.Leader = team.Leader;
            existingTeam.LeaderId = team.LeaderId;
            existingTeam.TeamName = team.TeamName;
            existingTeam.Employees = team.Employees;
            _context.Teams.Update(existingTeam);
            return _context.SaveChanges();
        }
    }
}
