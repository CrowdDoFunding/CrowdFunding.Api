using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Models;

namespace CrowdFundingTest.Options
{
    public class SearchProjectOptions
    {
        public int Id { get; set; }
        public ProjectCategory Category { get; set; }
        public ProjectState State { get; set; }
        public PackageCategory RewardPackageCategory { get; set; }
        public string Title { get; set; }
        public int CreatorId { get; set; }
        public DateTimeOffset StartDay{ get; set;}
        public DateTimeOffset LastDay { get; set; }
        public string Description { get; set; }
    }
}
