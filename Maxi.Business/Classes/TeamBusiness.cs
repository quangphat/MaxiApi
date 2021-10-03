using Maxi.Business.Interfaces;
using Maxi.Models.Infrastructures;
using Maxi.Models.Requests;
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
    public class TeamBusiness: BaseBusiness,ITeamBusiness
    {
        protected readonly ITeamRepository _rpTeam;
        public TeamBusiness(ITeamRepository teamRepository, CurrentProcess currentProcess):base(currentProcess)
        {
            _rpTeam = teamRepository;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existTeam = await _rpTeam.GetTeamByIdAsync(id);
            if (existTeam == null)
            {
                AddError("Dữ liệu không hợp lệ");
                return false;
            }

            await _rpTeam.DeleteAsync(existTeam);
            return true;
        }

        public async Task<int> CreateAsync(UpdateTeam model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                AddError("Dữ liệu không hợp lệ");
                return 0;
            }

            var team = new Team
            {
                LeaderId = model.LeaderId,
                Name = model.Name,
                ParentTeamId = model.ParentId
            };

            await _rpTeam.CreateAsync(team);
            return team.Id;

        }

        public async Task<bool> UpdateAsync(UpdateTeam model)
        {
            if(model==null || model.Id<1 || string.IsNullOrWhiteSpace(model.Name))
            {
                AddError("Dữ liệu không hợp lệ");
                return false;
            }

            var existTeam = await _rpTeam.GetTeamByIdAsync(model.Id);
            if(existTeam==null)
            {
                AddError("Dữ liệu không hợp lệ");
                return false;
            }

            existTeam.Name = model.Name;
            existTeam.ParentTeamId = model.ParentId;
            existTeam.LeaderId = model.LeaderId;

            await _rpTeam.UpdateAsync(existTeam);
            return true;
            
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
