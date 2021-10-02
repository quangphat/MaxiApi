using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Repository.Classes
{
    public abstract class RepositoryBase
    {
        protected readonly MaxiCorpContext _context;
        public RepositoryBase(MaxiCorpContext context)
        {
            _context = context;
        }
    }
}
