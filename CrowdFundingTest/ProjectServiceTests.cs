using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using CrowdFundingTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using test.Options;
using Xunit;

namespace CrowdFundingTest
{
    public class ProjectServiceTests: 
        IClassFixture<CrowdFundingFixture>
    {
        private CrowdFundingDbContext context_;
        private IUserService user_;
        private IProjectService proj_;
        public ProjectServiceTests(CrowdFundingFixture fixture)
        {
            context_ = fixture.Context;
            user_ = fixture.Users;
            proj_ = fixture.Projects;
        }

        [Fact]
        public void CreateProject_Success()
        {
            var options = new CreateUserOptions()
            {
                Email = "alkibiadis@gmail.com",
                VatNumber = "456325874",
                Age = 25,
                Name = "alkibiadis",
                Moto = "just keep moving"
            };

            var us = user_.CreateUser(options).Data;
            var popt = new CreateProjectOptions()
            {
                Category = ProjectCategory.Food,
                CreatorId = us.Id,
                Description = "the best food ever",
                Goal = 1500,
                Title = "Enjoy"
            };

            var pr = proj_.CreateProject(popt);

            Assert.NotNull(pr.Data);
            var query = context_.Set<Project>()
                .AsQueryable()               
                .Where(p => p.CreatorId == us.Id)
                .ToList();
            Assert.Single(query);

        }
    
        [Fact]
        public void CreateProject_SameTitle_Fail()
        {
            var opt = new CreateProjectOptions()
            {
                Title = "enjoy"
            };
            // the title was used above 
            var pr = proj_.CreateProject(opt);
            Assert.Null(pr.Data);
        }
        [Fact]
        public void SearchProject_Success()
        {
            var options = new CreateUserOptions()
            {
                Email = "papanontas@gmail.com",
                VatNumber = "477325874",
                Age = 25,
                Name = "iwannis",
                Moto = "walk"
            };

            var us = user_.CreateUser(options).Data;
            var popt = new CreateProjectOptions()
            {
                Category = ProjectCategory.Game,
                CreatorId = us.Id,
                Description = "play with friends",
                Goal = 1500,
                Title = "cool games"
            };

            var pr = proj_.CreateProject(popt);

            var sr = new SearchProjectOptions()
            {
                Category = ProjectCategory.Game,
                CreatorId = us.Id,
                Title = "cool games"
            };

            var pro = proj_.SearchProject(sr);

            Assert.NotNull(pro.Data);
        }
        [Fact]
        public void UpdateProject_Success()
        {
            var options = new CreateUserOptions()
            {
                Email = "eleni@gmail.com",
                VatNumber = "499325874",
                Age = 25,
                Name = "eleni",
                Moto = "smile"
            };

            var us = user_.CreateUser(options).Data;
            var popt = new CreateProjectOptions()
            {
                Category = ProjectCategory.Technology,
                CreatorId = us.Id,
                Description ="technology rocks",
                Goal = 1900,
                Title = "cool toys"
            };

            var pr = proj_.CreateProject(popt);

            var up = new UpdateProjectOptions()
            {
                Category = ProjectCategory.Games,
                ProjectId = pr.Data.Id
            };

            var uppro = proj_.UpdateProject(up);
            Assert.Equal(ProjectCategory.Games, uppro.Data.Category);

        }


    
    }
}
