using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MutualFundNAVTrigger.NETWebAPI.Models;
using System.Collections.Generic;
namespace MutualFundNAVTrigger.NETWebAPI.Data
{
   
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options) { }

            
       
        }
    }

