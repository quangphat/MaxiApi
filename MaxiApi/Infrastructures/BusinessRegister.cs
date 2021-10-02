using Maxi.Business.Classes;
using Maxi.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaxiApi.Infrastructures
{
    public static class BusinessRegister
    {
        public static void RegisterBusiness(this IServiceCollection service)
        {
            service.AddScoped<ITeamBusiness, TeamBusiness>();
        }
    }
}
