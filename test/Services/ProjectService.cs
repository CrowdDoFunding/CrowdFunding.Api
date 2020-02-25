using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Options;

namespace CrowdFundingTest.Services
{
    public class ProjectService : IProjectService
    {
        private CrowdFundingDbContext context_;
        private IUserService user_;
        public ProjectService(CrowdFundingDbContext context, IUserService user)
        {
            context_ = context;
            user_ = user;
        }

        public List<Project> GetProject(int howMany)
        {
            return context_.Set<Project>()
                .AsQueryable()
                .Take(howMany)
                .ToList();
        }
        public List<Project> GetProjectByCategory(SearchProjectOptions options)
        {
            return context_.Set<Project>()
                .AsQueryable()
                .Where(p => p.Category == options.Category)
                .ToList();
        }
        public ApiResult<Project> GetProjectById(int id)
        {
            var project = context_
                .Set<Project>()
                .SingleOrDefault(u => u.Id == id);

            if (project == null)
            {
                return new ApiResult<Project>(
                    StatusCode.NotFound,
                    $"Project {id} not found");
            }
            return ApiResult<Project>.CreateSuccessful(project);
        }
        public List<Project> GetProjectByCreator(SearchProjectOptions options)
        {
            return context_.Set<Project>()
                .AsQueryable()
                .Where(p => p.CreatorId == options.CreatorId)
                .ToList();
        }
        public ApiResult<IQueryable<Project>> SearchProject(
            SearchProjectOptions options)
        {
            if (options == null)
            {
                return ApiResult<IQueryable<Project>>.CreateUnSuccessful(
                    StatusCode.NoContext,
                    "null options");
            }
            if (
                options.Category == ProjectCategory.Invalid
                && options.Id == 0
                && string.IsNullOrWhiteSpace(options.Title)
                && options.State == ProjectState.Deactive
                && options.CreatorId == default
                && options.StartDay == default(DateTimeOffset)
                && options.LastDay == default(DateTimeOffset))
            {
                return ApiResult<IQueryable<Project>>.CreateUnSuccessful(
                    StatusCode.NoContext,
                    "at least one option must be applied");
            }
            var query = context_.Set<Project>()
                                .AsQueryable();
            //search by Id 
            if (options.Id != 0)
            {
                query = query.Where(p =>
                     p.Id == options.Id);
            }
            //search by projectcategory
            if (options.Category != ProjectCategory.Invalid)
            {
                query = query.Where(p =>
                       p.Category == options.Category);
            }
            //search by title
            if (options.Title != null)
            {
                query = query.Where(p =>
                       p.Title.Equals(options.Title,
                       StringComparison.OrdinalIgnoreCase));
            }
            //search the active projects
            if (options.State != ProjectState.Deactive)
            {
                query = query.Where(p =>
                       p.State == options.State);
            }
            //search by ceatorId
            if (options.CreatorId != default)
            {
                query = query.Where(p =>
                     p.CreatorId == options.CreatorId);
            }
            //search by date - Date Range
            if (options.StartDay != null)
            {
                query = query.Where(p => p.CreatedDate <= options.StartDay);
            }
            if (options.LastDay != null)
            {
                query = query.Where(p => p.CreatedDate >= options.LastDay);
            }
            return ApiResult<IQueryable<Project>>.CreateSuccessful(query);
        }
        public ApiResult<Project> CreateProject(CreateProjectOptions options)
        {
            var user = user_.GetUserById(options.CreatorId);
            //options.CreatorId = user.Data.Id;
            if (options == null)
            {
                return ApiResult<Project>.CreateUnSuccessful
                    (StatusCode.NoContext, "null options");
            }

            if (string.IsNullOrWhiteSpace(options.Title)
                && string.IsNullOrWhiteSpace(options.Description)
                && (options.Goal <= 0)
                && (options.CreatorId == 0 )
                && (options.Category == ProjectCategory.Invalid))
            {
                return ApiResult<Project>.CreateUnSuccessful
                   (StatusCode.BadRequest, "invalid options");
            }
            //making sure that the title is not given again
                var proj = context_
                    .Set<Project>()
                    .Where(p => p.Title == options.Title)
                    .Any();
                //check if projectTitle exists
                if (proj)
                {
                return ApiResult<Project>.CreateUnSuccessful(
                        StatusCode.BadRequest,
                        "the title already exists");
                }
                var project = new Project();
                project.Title = options.Title;
                project.Description = options.Description;
                project.Category = options.Category;
                project.Goal = options.Goal;
                project.CreatedDate = DateTimeOffset.Now.Date;

                project.CreatorId = user.Data.Id; // From Route
                context_.Set<Project>().Add(project);
                context_.SaveChanges();
               return ApiResult<Project>.CreateSuccessful(project);
        }
            
        public ApiResult<Project> UpdateProject(UpdateProjectOptions options)
        {
            if (options == null)
            {
                return ApiResult<Project>.CreateUnSuccessful
                    (StatusCode.BadRequest,
                    "null options");
            }
            var searchoptions = new SearchProjectOptions()
            {
                Id = options.ProjectId
            };
            var project = SearchProject(searchoptions)
                           .Data
                           .SingleOrDefault();
            if (project == null)
            {
                return ApiResult<Project>.CreateUnSuccessful(
                    StatusCode.BadRequest, 
                    $"this {options.ProjectId} does not exist!");
            }
            if (!string.IsNullOrWhiteSpace(options.Description)
                && !project.Description
                           .Equals(
                            options.Description,
                            StringComparison.OrdinalIgnoreCase))
            {
                project.Description = options.Description;
            }
            var proj = context_.
                      Set<Project>().
                      Where(p => 
                      p.Title == options.Title)
                      .Any();
            if (!string.IsNullOrWhiteSpace(options.Title)
                && !project.Title
                    .Equals(
                     options.Title,
                     StringComparison.OrdinalIgnoreCase)
                && !proj)
            {
                project.Title = options.Title;
            }
            if (options.Category != ProjectCategory.Invalid
                && project.Category != options.Category)
            {
                project.Category = options.Category;
            }
            if (project.State != options.State
                && options.State != ProjectState.Deactive)
            {
                project.State = options.State;
            }
            if (project.CashState != options.CashState
                && options.CashState != CashDeskState.NotFunded)
            {
                project.CashState = options.CashState;
            }
            if (options.Goal > 0 &&
                project.Goal != options.Goal)
            {
                project.Goal = options.Goal;
            }
            context_.SaveChanges();
            return ApiResult<Project>.CreateSuccessful(project);
        }
    }
}