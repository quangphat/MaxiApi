using Maxi.Business.Interfaces;
using Maxi.Models.Infrastructures;
using Maxi.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : MaxiBaseController
    {
        private readonly ILogger<EmployeesController> _logger;
        protected readonly IEmployeeBusiness _bzEmployee;

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeBusiness employeeBusiness, CurrentProcess currentProcess) : base(currentProcess)
        {
            _logger = logger;
            _bzEmployee = employeeBusiness;
        }
        [HttpGet]
        public async Task<IActionResult> Gets(string freeText, int page = 1, int limit = 20,DateTime? fromDate =null, DateTime? toDate= null)
        {
            var result = await _bzEmployee.Gets(freeText, page, limit, fromDate ,toDate);
            return ToResponse(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _bzEmployee.GetByIdAsync(id);
            return ToResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeModel model)
        {
            var result = await _bzEmployee.UpdateAsync(model);
            return ToResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpdateEmployeeModel model)
        {
            var result = await _bzEmployee.CreateAsync(model);
            return ToResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bzEmployee.DeleteAsync(id);
            return ToResponse(result);
        }
    }
}
