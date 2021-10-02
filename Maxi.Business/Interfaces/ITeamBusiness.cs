using Maxi.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Business.Interfaces
{
    public interface ITeamBusiness
    {
        Task<List<Team>> Gets();
    }
}
