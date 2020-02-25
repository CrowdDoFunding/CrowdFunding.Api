using CrowdFundingTest.Models;

namespace test.Services
{
    public class CreatePackageOptions
    {
        public string Description { get; set; }
        public PackageCategory Category { get; set; }
        public int ProjectId { get; set; }
        public decimal MinFunding { get; set; }
        public decimal MaxFunding { get; set; }
    }
}