using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using CrowdFundingTest.Services;
using System;
using System.Collections.Generic;
using System.Text;
using test.Services;
using Xunit;

namespace CrowdFundingTest
{
    public class PackageServiceTests:
        IClassFixture<CrowdFundingFixture>
    {
        private CrowdFundingDbContext context_;
        private IUserService user_;
        private IProjectService proj_;
        private IPackageService pack_;
        public PackageServiceTests(CrowdFundingFixture fixture)
        {
            context_ = fixture.Context;
            user_ = fixture.Users;
            proj_ = fixture.Projects;
            pack_ = fixture.Packages;
        }

      [Fact]
    public void CreatePackage_Success()
        {
            var options = new CreateUserOptions()
            {
                Email = "marina@gmail.com",
                VatNumber = "739325874",
                Age = 25,
                Name = "marina",
                Moto = "dance"
            };
            var us = user_.CreateUser(options).Data;
            var popt = new CreateProjectOptions()
            {
                Category = ProjectCategory.Technology,
                CreatorId = us.Id,
                Description = "technology top",
                Goal = 1907,
                Title = "cool game boys"
            };
            var pr = proj_.CreateProject(popt);
            var opt = new CreatePackageOptions()
            {
                Description = "get to know us better",
                Category = PackageCategory.BecomeMember,
                MinFunding = 50,
                MaxFunding = 120,
                ProjectId = pr.Data.Id
            };
            var pack = pack_.CreatePackage(opt);
            Assert.NotEqual(0, pack.Data.Id);
        }
    }

}
