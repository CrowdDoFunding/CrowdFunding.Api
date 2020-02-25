using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Models;

namespace test.Options
{
    public class CreateFundingOptions
    {
        public int Id { get; set; }
        public decimal Cash { get; set; }
        public int BackerId { get; set; }
        public int PackageId { get; set; }
       
    }
}
