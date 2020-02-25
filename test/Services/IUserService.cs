using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CrowdFundingTest.Services
{
    public interface IUserService
    {
        List<Project> UserList(int creatorid);
        ApiResult<User> CreateUser(CreateUserOptions options);
        ApiResult<IQueryable<User>> SearchUser(SearchUserOptions options);
        ApiResult<List<User>> GetUsers(int howMuch);
        public ApiResult<User> GetUserById(int id);
        ApiResult<User> GetUserByVatNumber(string vatnumber);
        ApiResult<User> UpDateUser( UpdateUserOptions options);
    }
}
