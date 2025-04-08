using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Configuration;

namespace GestionVoituresExpress.Models
{

        
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Repairing> Repairing { get; set; }
        public DbSet<RepairingType> RepairingType { get; set; }
        public DbSet<RepairingAndType> RepairingAndType { get; set; }
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

            //Relation Car Reparation
            modelBuilder.Entity<Repairing>()
           .HasOne(r => r.Car)
           .WithMany(c=>c.Repairing) 
           .HasForeignKey(r => r.CarID)
           .OnDelete(DeleteBehavior.Cascade);

           //Relation Repairing=RepairingType Many to many
         
             modelBuilder.Entity<RepairingAndType>()
            .HasKey(rat => new { rat.RepairingId, rat.RepairingTypeId });

            modelBuilder.Entity<RepairingAndType>()
                .HasOne(rat => rat.Repairing)
                .WithMany(r => r.RepairingAndTypes)
                .HasForeignKey(rat => rat.RepairingId);

            modelBuilder.Entity<RepairingAndType>()
                .HasOne(rat => rat.RepairingType)
                .WithMany(rt => rt.RepairingAndTypes)
                .HasForeignKey(rat => rat.RepairingTypeId);

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
