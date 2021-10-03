﻿using Maxi.Models.Responses;
using Maxi.Repository;
using Maxi.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maxi.Business.Interfaces
{
    public interface ITeamBusiness
    {
        Task<Pagination<USPTeam>> Gets(string freeText, int page = 1, int limit = 20);
        Task<USPTeam> GetByIdAsync(int id);
    }
}
