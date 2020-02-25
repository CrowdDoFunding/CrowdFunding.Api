
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using test.Options;
using test.Services;

namespace CrowdFundingTest.Services
{
    public class FundService : IFundService
    {
        private IUserService user_;
        private Data.CrowdFundingDbContext context_;
        private IProjectService project_;
        private IPackageService package_;
        public FundService(
            CrowdFundingDbContext context,
            IUserService user,
            IProjectService project,
            IPackageService package)
        {
            context_ = context;
            user_ = user;
            project_ = project;
            package_ = package;
        }
        public ApiResult<Funding> CreateFunding(CreateFundingOptions options)
        {
            if (options == null)
            {
                return ApiResult<Funding>
                    .CreateUnSuccessful(StatusCode.NoContext, "Null options");
            }
            if (options.BackerId == default)
            {
                return ApiResult<Funding>
                    .CreateUnSuccessful(StatusCode.NoContext, "Empty User");
            }
            var searchuser = context_
                .Set<User>()
                .Where(u => u.Id == options.BackerId);
            if (!searchuser.Any())
            {
                return ApiResult<Funding>
                     .CreateUnSuccessful(StatusCode.NoContext,
                     "No such user");
            }
            //Look up for the package
            var package = context_
                 .Set<Package>()
                 .Where(p => p.Id == options.PackageId);
            if (!package.Any())
            {
                return ApiResult<Funding>
                        .CreateUnSuccessful(
                        StatusCode.NoContext,
                        "Package not found");
            }

            if (options.Cash <= 0)
            {
                return ApiResult<Funding>
                    .CreateUnSuccessful(
                    StatusCode.BadRequest,
                    "Add cash");
            }

            if (options.Cash < package.SingleOrDefault().MinFunding)
            {
                return ApiResult<Funding>
                    .CreateUnSuccessful(
                    StatusCode.BadRequest,
                    "Not enough cash");
            }

            if (!package.SingleOrDefault().State)
            {
                return ApiResult<Funding>
                    .CreateUnSuccessful(
                    StatusCode.BadRequest, 
                    "Not available package");
            }

            var proj = context_.Set<Project>()
                .Where(p => p.Id == package
                  .SingleOrDefault()
                  .ProjectId);

            if (proj.Any())
            {
                if ((proj.SingleOrDefault().Wallet + options.Cash) == 
                    proj.SingleOrDefault().Goal)
                {
                    proj.SingleOrDefault().Wallet =
                      proj.SingleOrDefault().Wallet + options.Cash;

                    proj.SingleOrDefault().State = ProjectState.Deactive;
                }

                if ((proj.SingleOrDefault().Wallet + options.Cash)
                    < proj.SingleOrDefault().Goal)
                {
                    proj.SingleOrDefault().Wallet =
                        proj.SingleOrDefault().Wallet + options.Cash;
                    if (proj.SingleOrDefault().CashState == 0)
                    {
                        proj.SingleOrDefault().CashState = CashDeskState.Funded;
                    }
                }
            }
            var fund = new Funding()
            {
                BackerId = searchuser.SingleOrDefault().Id,
                Cash = options.Cash,
                Package = package.SingleOrDefault()
                
            };
            var p = proj.ToList();
            p[0].Funds.Add(fund);
            context_.Add(fund);
            context_.SaveChanges();
            return ApiResult<Funding>.CreateSuccessful(fund);
        }

        public ApiResult<IQueryable<Funding>> SearchFunding(SearchFundingOptions options)
        {
            if (options == null)
            {
                return ApiResult<IQueryable<Funding>>
                    .CreateUnSuccessful(
                    StatusCode.BadRequest, 
                    "Null options");
            }
            if (options.BackerId == default
                && options.ProjectId == 0
                && options.MinCash <= 0
                && options.MaxCash<=0) {
                return ApiResult<IQueryable<Funding>>
                    .CreateUnSuccessful(
                    StatusCode.BadRequest, 
                    "At least one option is needed");
            }
            var searchfun = context_.Set<Funding>().AsQueryable();
            if (options.MaxCash> 0) {

                searchfun = searchfun
                    .Where(p => p.Cash <= options.MaxCash);
            }
            if (options.MinCash > 0) {
                searchfun = searchfun 
                    .Where(p => p.Cash >= options.MinCash);
            }
            if (options.ProjectId != 0){
                searchfun = searchfun
                    .Where(p =>p.Package.ProjectId == options.ProjectId);
            }
            if (options.BackerId != default) {
                searchfun = searchfun
                    .Where(f => f.BackerId == options.BackerId);
            }
            return ApiResult<IQueryable<Funding>>.CreateSuccessful(searchfun);
        }

    }
}
