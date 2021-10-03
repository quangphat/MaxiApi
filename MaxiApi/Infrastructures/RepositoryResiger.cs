using Maxi.Repository.Classes;
using Maxi.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxiApi.Infrastructures
{
    public static class RepositoryResiger
    {
        public static void RegisterRepository(this IServiceCollection service)
        {
            service.AddScoped<ITeamRepository, TeamRepository>();
            service.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }
    }
}
