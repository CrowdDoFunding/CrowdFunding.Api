using System.Collections.Generic;
using System.Linq;
using CrowdFundingTest.Models;
using test.Options;

namespace test.Services
{
    public interface IPackageService
    {
        ApiResult<IQueryable<Package>> SearchPackage(SearchPackageOptions options);
        ApiResult<Package> CreatePackage(CreatePackageOptions options);
        ApiResult<Package> Update(UpdatePackageOptions options);
    }
}