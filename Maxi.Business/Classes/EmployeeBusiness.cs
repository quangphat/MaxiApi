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
    public class EmployeeBusiness :BaseBusiness ,IEmployeeBusiness
    {
        protected readonly IEmployeeRepository _rpEmployee;
        public EmployeeBusiness(IEmployeeRepository employeeRepository, CurrentProcess currentProcess):base(currentProcess)
        {
            _rpEmployee = employeeRepository;
        }

        public async Task<Pagination<USPEmployee>> Gets(string freeText, int page = 1, int limit = 20, DateTime? fromDate = null, DateTime? toDate = null)
        {
            BusinessExtensions.ProcessPaging(ref page, ref limit);
            var result = await _rpEmployee.Gets(freeText, page, limit,fromDate, toDate);
            if (result == null || !result.Any())
            {
                return BusinessExtensions.ToPaging<USPEmployee>(0, null);
            }
            return BusinessExtensions.ToPaging<USPEmployee>(result[0].TotalRecord, result);
        }

        public async Task<Pagination<USPEmployee>> GetTeamMebers(int teamId, int page = 1, int limit = 20)
        {
            BusinessExtensions.ProcessPaging(ref page, ref limit);

            var result = await _rpEmployee.GetTeamMemberAsync(teamId, page, limit);
            if (result == null || !result.Any())
            {
                return BusinessExtensions.ToPaging<USPEmployee>(0, null);
            }
            return BusinessExtensions.ToPaging<USPEmployee>(result[0].TotalRecord, result);
        }

        public async Task<USPEmployee> GetByIdAsync(int id)
        {
            if (id < 1)
                return null;

            return await _rpEmployee.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exist = await _rpEmployee.GetEmployeeByIdAsync(id);
            if (exist == null)
            {
                AddError("Dữ liệu không hợp lệ");
                return false;
            }

            await _rpEmployee.DeleteAsync(exist);
            return true;
        }

        public async Task<int> CreateAsync(UpdateEmployeeModel model)
        {
            if (model == null )
            {
                AddError("Dữ liệu không hợp lệ");
                return 0;
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                AddError("Vui lòng nhập họ tên");
                return 0;
            }

            if (model.TeamId < 1)
            {
                AddError("Vui lòng chọn team");
                return 0;
            }

            var employee = new Employee
            {
                FullName = model.FullName,
                Title = model.Title,
                Email = model.Email,
                Phone = model.Phone,
                TeamId = model.TeamId
            };

            await _rpEmployee.CreateAsync(employee);
            return employee.Id;
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeModel model)
        {
            if (model == null || model.Id < 1 )
            {
                AddError("Dữ liệu không hợp lệ");
                return false;
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                AddError("Vui lòng nhập họ tên");
                return false;
            }

            if (model.TeamId < 1)
            {
                AddError("Vui lòng chọn team");
                return false;
            }

            var exist = await _rpEmployee.GetEmployeeByIdAsync(model.Id);
            if (exist == null)
            {
                AddError("Dữ liệu không hợp lệ");
                return false;
            }

            exist.FullName = model.FullName;
            exist.Phone = model.Phone;
            exist.Email = model.Email;
            exist.Title = model.Title;
            exist.TeamId = model.TeamId;

            await _rpEmployee.UpdateAsync(exist);
            return true;
        }
    }
}
