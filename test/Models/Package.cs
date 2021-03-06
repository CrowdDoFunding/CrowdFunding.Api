﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public PackageCategory Category { get; set; }
        public int ProjectId { get; set; }
        public bool State { get; set; }
        public decimal MinFunding { get; set; }
        public decimal MaxFunding { get; set; }

        public Package()
        {
            State = true;
        }
    }
}
