using Maxi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Classes
{
    public class TeamRepository : RepositoryBase, ITeamRepository
    {
        public TeamRepository(MaxiCorpContext context) : base(context)
        {
        }

        public Task<List<Team>> Gets()
        {
            return _context.Teams.ToListAsync();
        }
    }
}
