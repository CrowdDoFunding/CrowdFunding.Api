using CrowdFundingTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Options
{
    public class UpdateProjectOptions
    {
        public int ProjectId { get; set; }
        public ProjectCategory Category { get; set; }
        public ProjectState State { get; set; }
        public string Title { get; set; }
        public CashDeskState CashState { get; set; }
        public decimal Goal { get; set; }
        public string Description { get; set; }
    }
}
