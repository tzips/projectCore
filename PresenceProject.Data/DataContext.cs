using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PresenceProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresenceProject.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<User> Users { get; set; }
        public DbSet<Presence> Presence { get; set; }
        public DbSet<LeaveRequest> Requests { get; set; }


        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // המרת DateOnly ל- DateTime
            modelBuilder.Entity<Presence>()
                .Property(p => p.Date)
                .HasConversion(
                    v => v.ToDateTime(new TimeOnly(0, 0)), // המרת DateOnly ל- DateTime
                    v => DateOnly.FromDateTime(v) // המרת DateTime ל- DateOnly
                );

            // המרת TimeOnly ל- TimeSpan
            modelBuilder.Entity<Presence>()
                .Property(p => p.Start)
                .HasConversion(
                    v => v.ToTimeSpan(), // המרת TimeOnly ל- TimeSpan
                    v => TimeOnly.FromTimeSpan(v) // המרת TimeSpan ל- TimeOnly
                );

            modelBuilder.Entity<Presence>()
                .Property(p => p.End)
                .HasConversion(
                    v => v.ToTimeSpan(), // המרת TimeOnly ל- TimeSpan
                    v => TimeOnly.FromTimeSpan(v) // המרת TimeSpan ל- TimeOnly
                );
        }
    }
}
