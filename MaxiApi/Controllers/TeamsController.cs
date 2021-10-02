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
    [Route("[controller]")]
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
        public async Task<List<Team>> Gets()
        {
            return await _bzTeam.Gets();
        }
    }
}
