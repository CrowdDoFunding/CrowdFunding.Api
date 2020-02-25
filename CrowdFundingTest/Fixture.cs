using CrowdFundingTest.Data;
using CrowdFundingTest.Services;
using System;
using System.Collections.Generic;
using System.Text;
using test.Services;
using Xunit;

namespace CrowdFundingTest
{
    public class CrowdFundingFixture : IClassFixture<CrowdFundingFixture>
    {
        public Data.CrowdFundingDbContext Context { get; private set; }
        public IUserService Users { get; private set; }
        public IProjectService Projects { get; private set; }
        public IPackageService Packages { get; private set; }
        public IFundService Funds { get; private set; }
        public CrowdFundingFixture()
        {
            Context = new Data.CrowdFundingDbContext();
            Users = new UserService(Context);

            Projects = new ProjectService(Context, Users);
            Packages = new PackageService(Context,
                                      Projects);
            Funds = new FundService(Context, Users, Projects,
                                         Packages);
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

 