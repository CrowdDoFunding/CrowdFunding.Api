using CrowdFundingTest.Data;
using CrowdFundingTest.Services;
using System;
using System.Collections.Generic;
using System.Text;
using test.Services;
using Xunit;

namespace CrowdFundingTest
{
    public class FundServiceTests:
                   IClassFixture<CrowdFundingFixture>
    {
        private CrowdFundingDbContext context_;
        private IUserService user_;
        private IProjectService proj_;
        private IPackageService pack_;

        public FundServiceTests(CrowdFundingFixture fixture)
        {
            context_ = fixture.Context;
            user_ = fixture.Users;
            proj_ = fixture.Projects;
            pack_ = fixture.Packages;
        }
    }
}
