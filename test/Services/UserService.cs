using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Services
{
    public class UserService: IUserService
    {
        private Data.CrowdFundingDbContext context_;
        public UserService(Data.CrowdFundingDbContext context)
        {
            context_ = context;
        }
        public ApiResult<List<User>> GetUsers(int howMuch)
        {
            var query = context_.Set<User>()
                        .AsQueryable()
                        .Take(howMuch)
                        .ToList();
            return ApiResult<List<User>>.CreateSuccessful(query);
        }

        public ApiResult<User> CreateUser(CreateUserOptions options)
        {
            var user = new User();
            if (options == null)
            {
                return ApiResult<User>
                    .CreateUnSuccessful(StatusCode.NoContext, "Empty options");
            }
            if (!Validations.EmailIsValid(options.Email)
                && !Validations.VatNumberIsValid(options.VatNumber)
                && string.IsNullOrWhiteSpace(options.Name)
                && options.Age < 18
                && string.IsNullOrWhiteSpace(options.Password)
                && string.IsNullOrWhiteSpace(options.Moto)
                )
            {
                return ApiResult<User>.CreateUnSuccessful(
                  StatusCode.BadRequest,
                  "no option were given");
            }
            var existUser = context_
                .Set<User>()
                .AsQueryable();
            if (existUser
                .Where(u => u.Email.Equals(options.Email)).Any())
            {
                return ApiResult<User>
                      .CreateUnSuccessful(StatusCode.BadRequest,
                      "Existed user options");
            }
             if (existUser.Where(u => u.VatNumber.Equals(options.VatNumber))
                .Any() )
            {
                return ApiResult<User>
                      .CreateUnSuccessful(StatusCode.BadRequest,
                      "Existed user options");
            }
            user.Moto = options.Moto;
            user.Name = options.Name;
            user.Email = options.Email;
            user.VatNumber = options.VatNumber;
            user.Age = options.Age;
            user.Password = options.Password;
            context_.Set<User>().Add(user);
            context_.SaveChanges();
            return ApiResult<User>.CreateSuccessful(user);
        }

        public ApiResult<IQueryable<User>> SearchUser(SearchUserOptions options) { 
            if (options == null)
            {
                return ApiResult<IQueryable<User>>.CreateUnSuccessful(
                   StatusCode.NoContext,
                   "null options");
            }
            if (options.Id == 0
              && string.IsNullOrWhiteSpace(options.VatNumber)
              && string.IsNullOrWhiteSpace(options.Name)
              && string.IsNullOrWhiteSpace(options.Email))
            {
                return ApiResult<IQueryable<User>>.CreateUnSuccessful(
                        StatusCode.NoContext,
                       "at least one options must be applied");
            }
            var query = context_.Set<User>()
                        .AsQueryable();
            if (!string.IsNullOrWhiteSpace(options.VatNumber))
            {
                query = query.Where(p => p.VatNumber == options.VatNumber);
            }
            if (options.Id != default)
            {
                query = query.Where(p => p.Id == options.Id);
            }
            if (!string.IsNullOrWhiteSpace(options.Name))
            {
                query = query.Where(p => p.Name.Equals(options.Name));
            }
            var users = query;
            return ApiResult<IQueryable<User>>.CreateSuccessful(users);
        }

        public ApiResult<User> UpDateUser(UpdateUserOptions options)
        {
            if (options == null)
            {
                return ApiResult<User>.CreateUnSuccessful(
                    StatusCode.NoContext,
                    "no options were given");
            }
            var searchuser = context_
                .Set<User>()
                .Where(u => u.Id == options.Id);

            var user = searchuser.SingleOrDefault();
            //check if the vatnumber or the mail given is already recorded
            var opt = new SearchUserOptions()
            {
                VatNumber = options.VatNumber, 
                Email = options.Email
            };
            var u1 = SearchUser(opt);
            if (u1.ErrorCode == StatusCode.Ok)
            {
                return ApiResult<User>.CreateUnSuccessful(
                     StatusCode.BadRequest,
                     "the vatnumber already exists");
            }
            if (!user.VatNumber.Equals(options.VatNumber)
                 && !string.IsNullOrWhiteSpace(options.VatNumber))
            {
                user.VatNumber = options.VatNumber;
            }
            if (!user.Email.Equals(options.Email) 
                     && !string.IsNullOrWhiteSpace(options.Email))
            {
                user.Email = options.Email;
            }
            if ( user.Age != options.Age && options.Age != 0)
            {
                user.Age = options.Age;
            }
            if (!user.Name.Equals(options.Name)
                && !string.IsNullOrWhiteSpace(options.Name))
            {
                user.Name = options.Name;
            }

            if (!user.Moto.Equals(options.Moto)
                   && !string.IsNullOrWhiteSpace(options.Moto))
            {
               user.Moto = options.Moto;
            }
            if (!user.Password.Equals(options.Password) 
                  && !string.IsNullOrWhiteSpace(options.Password))
            {
                user.Password = options.Password;
            }
            context_.SaveChanges();
            return ApiResult<User>.CreateSuccessful(user);
        }
        public ApiResult<User> GetUserById(int id)
        {
            var user = context_
                .Set<User>()
                .SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return new ApiResult<User>(
                    StatusCode.NotFound, 
                    $"User {id} not found");
            }
            return ApiResult<User>.CreateSuccessful(user);
        }
        public ApiResult<User> GetUserByVatNumber(string vatnumber)
        {
           if (string.IsNullOrWhiteSpace(vatnumber))
            {
                return ApiResult<User>.CreateUnSuccessful(
                    StatusCode.NoContext,
                    "no vatnumber was given");
            }
            var options = new SearchUserOptions()
            {
                VatNumber = vatnumber
            };
            var user = SearchUser(options)
                .Data
                .SingleOrDefault();
            return ApiResult<User>.CreateSuccessful(user);
        }
        public List<Project> UserList(int creatorid) 
        {
            var query = context_.Set<Project>()
                      .Where(p =>
                         p.CreatorId == creatorid)
                      .ToList();
            return query;
        }
    }
}
