using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using CrowdFundingTest.Services;
using test.Options;

namespace test.Services
{
    public class PackageService : IPackageService
    {
        readonly CrowdFundingDbContext context_;
        readonly IProjectService project_;
        public PackageService(
                     CrowdFundingDbContext cont,
                     IProjectService project)
        {
            context_ = cont;
            project_ = project;
        }

        public ApiResult<Package> CreatePackage
            (CreatePackageOptions options)
        {
            if (options == null)
            {
                return ApiResult<Package>
                    .CreateUnSuccessful(
                    StatusCode.NoContext,
                    "Null options");
            }
            if (options.ProjectId == 0
                 || options.ProjectId != context_
                     .Set<Project>()
                     .Where(p =>
                      p.Id == options.ProjectId)
                      .SingleOrDefault()
                      .Id)
            {
                return ApiResult<Package>
                    .CreateUnSuccessful
                    (StatusCode.NotFound,
                     "No such project");
            }

            if (options.Category == PackageCategory.Invalid)
            {
                return ApiResult<Package>
                    .CreateUnSuccessful
                    (StatusCode.BadRequest,
                    "Invalid Package");
            }

            if (options.MinFunding <= 0
                || options.MaxFunding <= 0)
            {
                return ApiResult<Package>
                    .CreateUnSuccessful
                    (StatusCode.BadRequest,
                    "Invalid Min-max value");
            }
            var results = new Package()
            {
                Description = options.Description,
                Category = options.Category,
                ProjectId = options.ProjectId,
                MinFunding = options.MinFunding,
                MaxFunding = options.MaxFunding
            };

            var projectOptions = new SearchProjectOptions()
            {
                Id = results.ProjectId
            };
            var project = project_.SearchProject(projectOptions)
                                  .Data
                                  .SingleOrDefault();
            context_.Set<Package>().Add(results);
            context_.SaveChanges();
            return ApiResult<Package>
                .CreateSuccessful(results);
        }

        public ApiResult<IQueryable<Package>>
            SearchPackage(SearchPackageOptions options)
        {
            if (options == null)
            {
                return ApiResult<IQueryable<Package>>
                    .CreateUnSuccessful(StatusCode.NoContext,
                    "Null options");
            }
            var query = context_.Set<Package>()
                .AsQueryable();
            if (query == context_.Set<Package>())
            {
                if (options.PackageId == 0
                    && options.Category == PackageCategory.Invalid
                    && options.ProjectId == 0
                    && !options.State
                    && string.IsNullOrWhiteSpace(options.WordForSearch)
                    && options.MinFunding <= 0
                    && options.MaxFunding <= 0)
                {
                    return ApiResult<IQueryable<Package>>
                        .CreateUnSuccessful(StatusCode.BadRequest
                        , "All the options invalid");
                }
            }

            if (options.PackageId != 0)
            {
                query = query.Where(p => p.Id == options.PackageId);
            }
            //Look up by string
            if (!string.IsNullOrWhiteSpace(options.WordForSearch))
            {
                query = query
                    .Where(p => p.Description
                    .Contains(options.WordForSearch,
                    StringComparison.OrdinalIgnoreCase));
            }
            if (options.Category == PackageCategory.Discount
                || options.Category == PackageCategory.Insights
                || options.Category == PackageCategory.BecomeMember)
            {
                query = query
                    .Where(p => p.Category == options.Category);
            }
            if (options.ProjectId != 0)
            {
                query = query.Where(p => p.Id == options.ProjectId);
            }
            return ApiResult<IQueryable<Package>>
                .CreateSuccessful(query);
        }

        public ApiResult<Package>
            Update(UpdatePackageOptions options)
        {
            if (options == null)
            {
                return ApiResult<Package>
                    .CreateUnSuccessful(StatusCode.NoContext,
                    "Null options");
            }

            if (options.PackageId == 0)
            {
                return ApiResult<Package>
                    .CreateUnSuccessful(StatusCode.BadRequest,
                    "A packageid is needed");
            }

            if (!string.IsNullOrWhiteSpace(options.Description)
                && options.Category == 0
                && options.MaxFunding == 0
                && options.MinFunding == 0)
            {
                return ApiResult<Package>
                    .CreateUnSuccessful(StatusCode.BadRequest,
                    "No options to update the package ");
            }
            var package = context_
                .Set<Package>()
                .Where(p => p.Id == options.PackageId);
            var trackedpackage = package.SingleOrDefault();
            if (package.Any())
            {

                if (!string.IsNullOrWhiteSpace(options.Description))
                {
                    trackedpackage.Description = options.Description;
                }
                if (options.Category != 0)
                {
                    trackedpackage.Category = options.Category;
                }

                if (options.MinFunding >= 0)
                {
                    trackedpackage.MinFunding = options.MinFunding;
                }

                if (options.MaxFunding > 0)
                {
                    trackedpackage.MaxFunding = options.MaxFunding;
                }

                if (options.State != trackedpackage.State)
                {
                    trackedpackage.State = options.State;
                }
                context_.SaveChanges();
            }

            return ApiResult<Package>.CreateSuccessful(trackedpackage);
        }
    }
}

