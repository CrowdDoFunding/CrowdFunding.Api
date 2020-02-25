using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdFundingTest.Models;
using Microsoft.EntityFrameworkCore;

namespace CrowdFundingTest.Data
{
    public class CrowdFundingDbContext : DbContext
    {
        private readonly string connection_;

        public CrowdFundingDbContext()
        { 
            connection_=
               connection_ =
                "Server = DESKTOP-UM5ND34; Database=KickDo-newTest; Integrated Security = SSPI; Persist Security Info=False;";
        }

        public CrowdFundingDbContext(string conection)
        {
            connection_ = conection;
        }
        
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>()
                .ToTable("User", "core");

            modelBuilder
                   .Entity<Project>()
                   .ToTable("Project", "core");

            modelBuilder
                .Entity<Funding>()
                .ToTable("Funding", "core");

            modelBuilder
                .Entity<Package>()
                .ToTable("Package", "core");

        }
                

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connection_);
        }

    }
}
