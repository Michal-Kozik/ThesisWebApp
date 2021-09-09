using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ThesisWebApp.Models;

// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1

namespace ThesisWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {    
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-ThesisWebApp-A8B0920F-AAAE-49E7-A4DF-2214F6BE4D5F;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ApplicationUser>()
            //    .HasOne(s => s.Statistics)
            //    .WithOne(i => i.ApplicationUser)
            //    .HasForeignKey<Statistics>(s => s.ApplicationUserID);
            //modelBuilder
            //    .Entity<Mark>()
            //    .HasOne(e => e.Exam)
            //    .WithMany(e => e.Marks)
            //    .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Statistics> Statistics { get; set; }

        public DbSet<Mark> Marks { get; set; }
    }
}
