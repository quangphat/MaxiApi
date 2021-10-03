using Maxi.Models.Requests;
using Maxi.Models.Responses;
using Maxi.Repository;
using Maxi.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Business.Interfaces
{
    public interface IEmployeeBusiness
    {
        Task<bool> DeleteAsync(int id);
        Task<int> CreateAsync(UpdateEmployeeModel model);
        Task<bool> UpdateAsync(UpdateEmployeeModel model);
        Task<Pagination<USPEmployee>> GetTeamMebers(int teamId, int page = 1, int limit = 20);
        Task<Pagination<USPEmployee>> Gets(string freeText, int page = 1, int limit = 20, DateTime? fromDate = null, DateTime? toDate = null);
        Task<USPEmployee> GetByIdAsync(int id);
    }
}
