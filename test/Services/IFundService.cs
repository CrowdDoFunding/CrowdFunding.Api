using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Models;
using test.Options;

namespace CrowdFundingTest.Services
{
    public interface IFundService
    {
        ApiResult<Funding> CreateFunding(CreateFundingOptions options);
        ApiResult<IQueryable<Funding>> SearchFunding(SearchFundingOptions options);
    }
}
