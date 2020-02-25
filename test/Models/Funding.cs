using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Models
{
    public class Funding
    {
        public int Id { get; set;  }
        public decimal Cash { get; set; }
        public int BackerId { get; set; }
         public Package Package { get; set; }
    }
}
