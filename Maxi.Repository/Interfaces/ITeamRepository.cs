using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>> Gets();
    }
}
