using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Models;

namespace test.Options
{
    public class SearchFundingOptions
    {
        public decimal MaxCash { get; set; }
        public decimal MinCash { get; set; }
        public int BackerId { get; set; }
        public int ProjectId { get; set; }

 
    }
   
}
