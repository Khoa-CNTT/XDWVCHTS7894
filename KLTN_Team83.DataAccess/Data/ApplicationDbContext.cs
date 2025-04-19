using KLTN_Team83.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KLTN_Team83.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions <ApplicationDbContext> options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<InfoUser> Users { get; set; }
        //public DbSet<MyImage> MyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //        //base.OnModelCreating(modelBuilder);
            //        // Configure the primary key for the User entity
            //        modelBuilder.Entity<User>()
            //            .HasKey(a => a.id_User);
            modelBuilder.Entity<Blog>().HasData(
                new Blog { id_Blog = 1, tilte = "user1", content = "user1@gmail.com", img = "nam.jpg" },
                new Blog { id_Blog = 2, tilte = "user2", content = "user1@gmail.com", img = "Nam.jpg" },
                new Blog { id_Blog = 3, tilte = "user3", content = "user1@gmail.com", img = "Healthy.jpg" }
                );
            modelBuilder.Entity<InfoUser>().HasData(
                new InfoUser {id_User=1, id_Account = 1, fullName = "Nguyễn Lê Trung Khánh", age = 22, gender = "Male", height=176, weight=84 }
                );
            //        // Configure the primary key for the Admin entity
            //        modelBuilder.Entity<Admin>()
            //            .HasKey(a => a.id_Admin);
            //        // Configure the primary key for the Blog entity
            //        modelBuilder.Entity<Blog>()
            //            .HasKey(b => b.id_Blog);
            //        // Configure the primary key for the Expert entity
            //        modelBuilder.Entity<Expert>()
            //            .HasKey(e => e.id_Expert);
            //        // Configure the primary key for the Blog entity
            //        modelBuilder.Entity<Blog>()
            //            .HasKey(a => a.id_Acc);
        }

    }
}
