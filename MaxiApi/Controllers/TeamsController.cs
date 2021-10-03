using Maxi.Business.Interfaces;
using Maxi.Models.Infrastructures;
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

        public TeamsController(ILogger<TeamsController> logger, ITeamBusiness teamBusiness, CurrentProcess currentProcess):base(currentProcess)
        {
            _logger = logger;
            _bzTeam = teamBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Gets(string freeText, int page = 1, int limit = 20)
        {
            var result=  await _bzTeam.Gets(freeText, page, limit);
            return ToResponse(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _bzTeam.GetByIdAsync(id);
            return ToResponse(result);
        }
    }
}
