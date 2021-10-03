using Maxi.Business.Interfaces;
using Maxi.Models.Infrastructures;
using Maxi.Models.Requests;
using Maxi.Repository;
using Maxi.Repository.Interfaces;
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
    public class TeamsController : MaxiBaseController
    {
        private readonly ILogger<TeamsController> _logger;
        protected readonly ITeamBusiness _bzTeam;
        protected readonly IEmployeeBusiness _bizEmployee;

        public TeamsController(ILogger<TeamsController> logger,
            IEmployeeBusiness employeeBusiness,
            ITeamBusiness teamBusiness, CurrentProcess currentProcess):base(currentProcess)
        {
            _logger = logger;
            _bzTeam = teamBusiness;
            _bizEmployee = employeeBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Gets(string freeText, int page = 1, int limit = 20)
        {
            var result=  await _bzTeam.Gets(freeText, page, limit);
            return ToResponse(result);
        }

        [HttpGet("{teamId}/members")]
        public async Task<IActionResult> Gets(int teamId, int page = 1, int limit = 20)
        {
            var result = await _bizEmployee.GetTeamMebers(teamId, page, limit);
            return ToResponse(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _bzTeam.GetByIdAsync(id);
            return ToResponse(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTeam model)
        {
            var result = await _bzTeam.UpdateAsync(model);
            return ToResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UpdateTeam model)
        {
            var result = await _bzTeam.CreateAsync(model);
            return ToResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bzTeam.DeleteAsync(id);
            return ToResponse(result);
        }
    }
}
