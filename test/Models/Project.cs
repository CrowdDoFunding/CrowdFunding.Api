using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatorId { get; set; }
        public ProjectState State { get; set; }
        public CashDeskState CashState { get; set; }
        public decimal Goal { get; set; }
        public decimal Wallet { get; set; }
        public List<Funding> Funds { get; set; }
        public List<Package> RewardPackages { get; set; }
        public Project()
        {
            State = ProjectState.Active;
            CashState = CashDeskState.NotFunded;
            Funds = new List<Funding>();
            RewardPackages = new List<Package>();
        }
    }
}
