using Maxi.Repository.Entities;
using Maxi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Classes
{
    public class TeamRepository : RepositoryBase, ITeamRepository
    {
        public TeamRepository(MaxiCorpContext context) : base(context)
        {
        }

        public async Task<List<USPTeam>> Gets(string freeText = null, int page = 1, int limit = 20)
        {
            return (await ExecQuery<USPTeam>("spGetTeams", new { freeText, page, limit }))?.ToList();
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            return await _context.Teams.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<USPTeam> GetByIdAsync(int id)
        {
            return (await ExecQuery<USPTeam>("spGetTeamById", new { id}))?.FirstOrDefault();
        }

        public async Task UpdateAsync(Team team)
        {
            team.UpdatedAt = DateTime.UtcNow;
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Team team)
        {
            team.CreatedAt = DateTime.UtcNow;
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Team team)
        {
            team.UpdatedAt = DateTime.UtcNow;
            team.IsDeleted = true;
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }
    }
}
