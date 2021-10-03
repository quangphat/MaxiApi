using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Repository.Classes
{
    public abstract class RepositoryBase
    {
        protected readonly MaxiCorpContext _context;
        public RepositoryBase(MaxiCorpContext context)
        {
            _context = context;
        }

        protected string buildStoreProcString(string query, object paras = null)
        {
            var paramString = string.Empty;

            if (paras != null)
            {
                paramString = string.Join(",", paras.GetType().GetProperties().Select(m => $"@{m.Name}").ToArray());
                paramString = $" {paramString}";
            }

            return $"exec {query}{paramString}";
        }

        protected async Task<IEnumerable<T>> ExecQuery<T>(string query, object paras = null)
        {
            var queryProce = buildStoreProcString(query, paras);
            var transaction = _context.Database.CurrentTransaction?.GetDbTransaction();

            var list = await _context.Database.GetDbConnection().QueryAsync<T>(queryProce, paras,transaction:transaction,commandType: System.Data.CommandType.Text);

            return list;
        }
    }
}
