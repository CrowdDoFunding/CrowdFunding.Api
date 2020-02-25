using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Models;

namespace CrowdFundingTest.Options
{
    public class CreateProjectOptions
    {
        public string Title{ get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
        public int CreatorId { get; set; }
        public decimal Goal { get; set; }
    }
}
