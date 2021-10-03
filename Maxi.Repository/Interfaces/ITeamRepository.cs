using Maxi.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<USPTeam>> Gets(string freeText = null, int page =1, int limit =20);
        Task<USPTeam> GetByIdAsync(int id);
    }
}
