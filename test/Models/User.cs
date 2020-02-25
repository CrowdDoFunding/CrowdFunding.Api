using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VatNumber { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Moto { get; set; }
        public string Password { get; set; }

       // public List<Project> Projects { get; set; }   
       // public List<Fund> Funds { get; set; }   
    }
   
}
