using Maxi.Business.Interfaces;
using Maxi.Models.Responses;
using Maxi.Repository;
using Maxi.Repository.Entities;
using Maxi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Pagination<USPTeam>> Gets(string freeText, int page =1, int limit = 20)
        {
            BusinessExtensions.ProcessPaging(ref page, ref limit);
            var result = await _rpTeam.Gets(freeText, page, limit);
            if(result==null || !result.Any())
            {
                return BusinessExtensions.ToPaging<USPTeam>(0, null);
            }
            return BusinessExtensions.ToPaging<USPTeam>(result[0].TotalRecord, result);
        }

        public async Task<USPTeam> GetByIdAsync(int id)
        {
            if (id < 1)
                return null;

            return await _rpTeam.GetByIdAsync(id);
        }
    }
}
