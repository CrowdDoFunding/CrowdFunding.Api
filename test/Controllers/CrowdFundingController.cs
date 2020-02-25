using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using CrowdFundingTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using test.Options;
using test.Services;

namespace test.Controllers
{
    [ApiController]

    [Route("[controller]")]
    public class CrowdFundingController : ControllerBase
    {
        private readonly CrowdFundingTest.Data.CrowdFundingDbContext context_;
        private readonly IUserService user_;
        private readonly IProjectService project_;
        private readonly ILogger<CrowdFundingController> _logger;
        private readonly IPackageService package_;
        private readonly IFundService fund_;
        private readonly IReportService report_;
        public CrowdFundingController(
            ILogger<CrowdFundingController> logger,
            CrowdFundingTest.Data.CrowdFundingDbContext cont,
            IProjectService proj,
            IUserService use,
            IPackageService package,
            IFundService fund,
            IReportService report)
        {
            fund_ = fund;
            context_ = cont;
            project_ = proj;
            user_ = use;
            _logger = logger;
            package_ = package;
            report_ = report;
        }
        [HttpGet]
        public string GetInfo()
        {
            return "Welcome to KickDo";
        }


        [HttpGet("userid/{id}")]
        public User GetUserById([FromRoute] int id)
        {
            return user_.GetUserById(id).Data;
        }

        [HttpGet("users")]
        public List<User> GetUsers()
        {
            return user_.GetUsers(100).Data;
        }

        [HttpGet("usersearch")]
        public List<User> GetUserSearch([FromBody] SearchUserOptions options)
        {
            return user_.SearchUser(options).Data.ToList();
        }

        [HttpPost("user")]
        public User CreateUser(
            [FromBody] CreateUserOptions options)
        {
            return user_.CreateUser(options).Data;
        }

        [HttpPut("user/{id}")]
        public User UpdateUser([FromRoute] int id,
             [FromBody] UpdateUserOptions options)
        {
            options.Id = id;
            return user_.UpDateUser(options).Data;
        }

        //ENDPOINTS for the Project
        [HttpGet("projects")]
        public List<Project> GetProjects()
        {
            return context_.Set<Project>().ToList();
        }
        [HttpGet("project/{id}")]
        public Project GetProjectById(
            [FromRoute] int id)
        {
            return project_.GetProjectById(id).Data;
        }
        [HttpGet("projectsearch")]
        public List<Project> GetProjectById(
            [FromBody] SearchProjectOptions options)
        {
            return project_.SearchProject(options).Data.ToList();
        } 

        [HttpGet("projects/funded")]
        public List<Project> GetProjectByState()
        {
            return context_.Set<Project>()
                .Where(p => p.State.Equals(CashDeskState.Funded)).ToList(); 
        }

        [HttpPost("project/{creatorId}")]
        public Project CreateProject(
              [FromBody] CreateProjectOptions options
             , [FromRoute] int creatorId)
        {
            options.CreatorId = creatorId;
            return project_.CreateProject(options).Data;
        }
        [HttpPut("project/{id}")]
        public Project UpdateProject([FromRoute] int id
             , [FromBody] UpdateProjectOptions options)
        {
            options.ProjectId = id;
            return project_.UpdateProject(options).Data;
        }
        //Packages
        [HttpPost("package")]
        public Package CreatePackage([FromBody] CreatePackageOptions options)
        {
           return package_.CreatePackage(options).Data;
        }

        [HttpGet("package")]
        public List<Package> GetPackage
            ([FromBody] SearchPackageOptions options)
        {
            return package_.SearchPackage(options).Data.ToList();
        }

        [HttpGet("packages")]
        public  List<Package> GetAllPackages()
        {
          return context_.Set<Package>().ToList();
        }

        [HttpPut("package/update")]
        public Package UpdatePackage(
            [FromBody] UpdatePackageOptions options)
        {
            return package_.Update(options).Data;
        }

        //FUNDINGS
        [HttpPost("funding")]
        public Funding CreateFund(
            [FromBody] CreateFundingOptions options)
        {
            return fund_.CreateFunding(options).Data;
        }

        [HttpGet("funding")]
        public List<Funding> GetFundings(
            [FromBody] SearchFundingOptions options)
        {
            return fund_.SearchFunding(options).Data.ToList();
        }
        [HttpGet("fundings")]
        public List<Funding> GetAllFundings()
        {
            return context_.Set<Funding>().ToList();
        }

        //ReportEndpoints

        [HttpGet("recentprojects")]
        public List<Project> GetRecentProjects()
        {
            return report_.RecentProject();
        }
        [HttpGet("top20creators")]
        public List<User> GetTop20Creators()
        {
            return report_.Top20Creator();
        }
        [HttpGet("top20projects")]
        public List<Project> GetTop20Projects()
        {
            return report_.Top20Projects();
        }
    }
}
