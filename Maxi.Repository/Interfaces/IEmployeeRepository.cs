using Maxi.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task DeleteAsync(Employee employee);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task CreateAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task<long> CountTeamMemberAsync(int teamId);
        Task<List<USPEmployee>> GetTeamMemberAsync(int teamId, int page = 1, int limit = 20);
        Task<List<USPEmployee>> Gets(string freeText = null, int page = 1, int limit = 20, DateTime? fromDate = null, DateTime? toDate = null);
        Task<USPEmployee> GetByIdAsync(int id);
    }
}
