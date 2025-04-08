using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GestionVoituresExpress.Models
{

        
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Relation Transaction  Car
                modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Car)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CarID)
                .OnDelete(DeleteBehavior.Cascade);
            //Relation Transaction Reparation
            modelBuilder.Entity<Repairing>()
           .HasOne(r => r.Transaction)
           .WithMany() 
           .HasForeignKey(r => r.TransactionId)
           .OnDelete(DeleteBehavior.Cascade);
            // Relation Transaction  User
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        }

     

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("AppDbContext");

            }
        }
    }

}
