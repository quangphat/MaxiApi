using Maxi.Repository.Entities;
using Maxi.Repository.Interfaces;
using Maxi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Classes
{
    public class EmployeeRepository : RepositoryBase, IEmployeeRepository
    {

        public EmployeeRepository(MaxiCorpContext context) : base(context)
        {

        }

        public async Task<List<USPEmployee>> GetTeamMemberAsync(int teamId, int page = 1, int limit= 20)
        {
            return (await ExecQuery<USPEmployee>("spGetTeamMembers", new { teamId, page, limit}))?.ToList();
        }

        public async Task<long> CountTeamMemberAsync(int teamId)
        {
            return _context.Employees.Where(p => p.TeamId == teamId).Count();
        }

        public async Task<List<USPEmployee>> Gets(string freeText = null, int page = 1, int limit = 20, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var now = DateTime.UtcNow;
            if (!fromDate.HasValue)
            {
                fromDate = now.AddDays(-60).ToStartDateTime();
            }
            if(!toDate.HasValue)
            {
                toDate = now.ToEndDateTime();
            }

            return (await ExecQuery<USPEmployee>("spGetEmployees", new { freeText, page, limit,fromDate, toDate }))?.ToList();
        }

        public async Task<USPEmployee> GetByIdAsync(int id)
        {
            return (await ExecQuery<USPEmployee>("spGetEmployeeById", new { id }))?.FirstOrDefault();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return _context.Employees.Where(p => p.Id == id).FirstOrDefault();
        }

        public async Task UpdateAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Employee employee)
        {

            var lastEntity = _context.Employees.OrderByDescending(p => p.Id).FirstOrDefault();

            var code = (await ExecQuery<string>("sp_generateCode", new { type="employee",id=lastEntity!=null?lastEntity.Id+1 :0 }))?.FirstOrDefault();
            employee.Code = code;
            employee.CreatedAt = DateTime.UtcNow;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            employee.UpdatedAt = DateTime.UtcNow;
            employee.IsDeleted = true;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
