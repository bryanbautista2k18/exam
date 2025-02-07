using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using api.Models;

namespace api.Data 
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<ConfigGender> ConfigGenders { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<User>()
                .Property(u => u.InsertedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
                .HasOne(mur => mur.ConfigGender)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            // Config Gender
            modelBuilder.Entity<ConfigGender>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<ConfigGender>()
                .Property(u => u.InsertedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ConfigGender>()
                .Property(u => u.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }
    }
}