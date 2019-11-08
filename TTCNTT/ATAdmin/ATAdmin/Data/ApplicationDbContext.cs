using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ATAdmin.Efs.Entities;

namespace ATAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<ATAdmin.Efs.Entities.AboutUs> AboutUs { get; set; }
    }
}
