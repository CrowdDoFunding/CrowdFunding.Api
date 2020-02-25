using CrowdFundingTest.Data;
using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using CrowdFundingTest.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CrowdFundingTest
{
    public class UserServiceTests :
         IClassFixture<CrowdFundingFixture>
    {

        private CrowdFundingDbContext context_;
        private IUserService user_;

        public UserServiceTests(CrowdFundingFixture fixture)
        {
            context_ = fixture.Context;
            user_ = fixture.Users;
        }

        [Fact]
        public void CreateUser_Success()
        {
            var options = new CreateUserOptions()
            {
                Age = 21,
                Email = "kk@gmail.com",
                Moto = "Keep Smiling",
                Name = "Konstantina Katsanou",
                VatNumber = "123651247"
            };
            var user = user_.CreateUser(options);

            Assert.NotNull(user.Data);
            Assert.NotEqual(0, user.Data.Id);

            
        }
        [Fact]
        public void UniqueEmail_success()
        {
            var options = new CreateUserOptions()
            {
                Email = "kk@gmail.com"
            };
            var user = user_.CreateUser(options);

            Assert.Null(user.Data);
        }

        [Fact]
        public void SearchUser_Success()
        {
            var options = new SearchUserOptions() { };
            // check if all values are equal with defaults
            var users = user_.SearchUser(options);
            Assert.Equal(StatusCode.NoContext, users.ErrorCode);

        }

        [Fact]
        public void Update_SameEmail_Fail()
        {
            var options = new CreateUserOptions()
            {
                Email = "marios@gmail.com",
                Name = "marios", 
                VatNumber = "977922444"
            };
            var user = user_.CreateUser(options).Data;
            var id = user.Id;
            var option = new UpdateUserOptions()
            {
                Name = user.Name,
                Id = id, 
                Email = "eleana@gmail.com"
            };
            var updateuser = user_.UpDateUser(option);

            Assert.Equal(StatusCode.BadRequest,
                updateuser.ErrorCode);

         }
    }

        
}
