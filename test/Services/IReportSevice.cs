using CrowdFundingTest.Models;
using CrowdFundingTest.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Services
{
    public interface IReportService
   {
        List<Project> Top20Projects();
        List<User> Top20Creator();
        List<Project> RecentProject();
    }
}
