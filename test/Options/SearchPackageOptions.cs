using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Models;

namespace test.Options
{
    public class SearchPackageOptions
    {
        public string WordForSearch { get; set; }
        public PackageCategory Category { get; set; }
        public int PackageId { get; set; }
        public int ProjectId { get; set; }
        public bool State { get; set; }
        public decimal MinFunding { get; set; }
        public decimal MaxFunding { get; set; }
    }
}
