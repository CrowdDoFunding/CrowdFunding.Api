using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Options;

namespace CrowdFundingTest.Services
{
    public interface IProjectService
    {
        ApiResult<Project> GetProjectById(int id);
        List<Project> GetProjectByCreator(SearchProjectOptions options);
        ApiResult<Project> CreateProject(CreateProjectOptions options );
        ApiResult<Project> UpdateProject(UpdateProjectOptions options);
        List<Project> GetProject(int howMany);
        List<Project> GetProjectByCategory(SearchProjectOptions options);
        ApiResult<IQueryable<Project>> SearchProject(SearchProjectOptions options);
    }
}
