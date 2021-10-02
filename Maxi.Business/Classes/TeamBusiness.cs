using Maxi.Business.Interfaces;
using Maxi.Repository;
using Maxi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Business.Classes
{
    public class TeamBusiness:ITeamBusiness
    {
        protected readonly ITeamRepository _rpTeam;
        public TeamBusiness(ITeamRepository teamRepository)
        {
            _rpTeam = teamRepository;
        }

        public async Task<List<Team>> Gets()
        {
            return await _rpTeam.Gets();
        }
    }
}
