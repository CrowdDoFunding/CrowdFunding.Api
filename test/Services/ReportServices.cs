using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using CrowdFundingTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Services
{
    public class ReportServices: IReportService
    {
        private IUserService user_;
        private CrowdFundingDbContext context_;
        private IProjectService project_;
        private IPackageService package_;
        private IFundService fund_;
        public ReportServices(
            CrowdFundingDbContext context,
            IUserService user,
            IProjectService project,
            IPackageService package, 
            IFundService fund)
        {
            context_ = context;
            user_ = user;
            project_ = project;
            package_ = package;
            fund_ = fund;
        }
      
        public List<Project> RecentProject()
        {
            var query = context_.Set<Project>()
                .AsQueryable()
                .Where(p =>
                p.CreatedDate.DateTime >=
                             DateTime.Today.AddDays(-7)
               &&  p.CreatedDate.Date <= 
                              DateTime.Today)
                .ToList();
            return query;
        }
       
        public List<Project> Top20Projects()
        {
            return  context_.Set<Project>()
                            .OrderBy(p => (p.Goal - p.Wallet))
                            .Take(20)
                            .ToList();
        }

        public List<User> Top20Creator()
        {
            var projects = Top20Projects().AsQueryable();
            var users = new List<User>();
            foreach( var pr in projects)
            {
                users.Add(context_.Set<User>()
                   .Find(pr.CreatorId));
            }
            return users;
        }
    }
}
