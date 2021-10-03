using Maxi.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Interfaces
{
    public interface ITeamRepository
    {
        Task CreateAsync(Team team);
        Task<Team> GetTeamByIdAsync(int id);
        Task UpdateAsync(Team team);
        Task<List<USPTeam>> Gets(string freeText = null, int page =1, int limit =20);
        Task<USPTeam> GetByIdAsync(int id);
        Task DeleteAsync(Team team);
    }
}
