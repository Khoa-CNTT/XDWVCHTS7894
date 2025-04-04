using KLTN_Team83.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KLTN_Team83.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions <ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Account> Accounts { get; set; }

        //    protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //        //base.OnModelCreating(modelBuilder);
        //        // Configure the primary key for the User entity
        //        modelBuilder.Entity<User>()
        //            .HasKey(a => a.id_User);
        //        modelBuilder.Entity<User>().HasData(
        //            new User { id_User = 1, id_Acc = 1, userName = "user1", email = "user1@gmail.com", gender = "Nam", height = 174, weight = 76 },
        //            new User { id_User = 2, id_Acc = 2, userName = "user2", email = "user2@gmail.com", gender = "Nam", height = 174, weight = 76 },
        //            new User { id_User = 3, id_Acc = 3, userName = "user3", email = "user3@gmail.com", gender = "Nữ", height = 174, weight = 76 }
        //            );
        //        // Configure the primary key for the Admin entity
        //        modelBuilder.Entity<Admin>()
        //            .HasKey(a => a.id_Admin);
        //        // Configure the primary key for the Blog entity
        //        modelBuilder.Entity<Blog>()
        //            .HasKey(b => b.id_Blog);
        //        // Configure the primary key for the Expert entity
        //        modelBuilder.Entity<Expert>()
        //            .HasKey(e => e.id_Expert);
        //        // Configure the primary key for the Account entity
        //        modelBuilder.Entity<Account>()
        //            .HasKey(a => a.id_Acc);
        //    }

    }
}
