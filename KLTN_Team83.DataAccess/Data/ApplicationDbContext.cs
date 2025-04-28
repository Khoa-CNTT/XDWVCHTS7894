using KLTN_Team83.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KLTN_Team83.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext( DbContextOptions <ApplicationDbContext> options) : base(options) { }

        public DbSet<TypeBlog> TypeBlogs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<InfoUser> Users { get; set; }
        //public DbSet<Expert> Experts { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<MyImage> MyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //        //base.OnModelCreating(modelBuilder);
            //        // Configure the primary key for the User entity
            //        modelBuilder.Entity<User>()
            //            .HasKey(a => a.id_User);
            modelBuilder.Entity<Category>().HasData(
                new Category { DisplayOrder = 1, Id_Category = 1, Name = "Thức ăn" },
                new Category { DisplayOrder = 2, Id_Category = 2, Name = "Thức uống" },
                new Category { DisplayOrder = 3, Id_Category = 3, Name = "Thức ăn dinh dưỡng" },
                new Category { DisplayOrder = 4, Id_Category = 4, Name = "Thực phẩm hỗ trợ" }
                );
            modelBuilder.Entity<TypeBlog>().HasData(
                new TypeBlog { id_TypeBlog = 1, Name = "Food", Description = "Thông tin về thức ăn dinh dưỡng" },
                new TypeBlog { id_TypeBlog = 2, Name = "Health", Description = "Thông tin về sức khỏe" },
                new TypeBlog { id_TypeBlog = 3, Name = "Exercises", Description = "Thông tin về các bài tập" },
                new TypeBlog { id_TypeBlog = 4, Name = "Mental", Description = "Thông tin về sức khỏe tinh thần" },
                new TypeBlog { id_TypeBlog = 5, Name = "Sleep", Description = "Thông tin về giấc ngủ" },
                new TypeBlog { id_TypeBlog = 6, Name = "Diet", Description = "Thông tin về chế độ ăn uống" }
                );
            modelBuilder.Entity<Blog>().HasData(
                new Blog { id_Blog = 1, id_TypeBlog = 1, tilte = "Rau củ", content = "Rau củ quả tốt cho hệ tiêu hóa", img = "raucu.jpg" },
                new Blog { id_Blog = 2, id_TypeBlog = 1, tilte = "Uống nước", content = "Uống nước giúp cơ thể lưu thông máu tốt hơn", img = "nuoc.jpg" },
                new Blog { id_Blog = 3, id_TypeBlog = 3, tilte = "Bơi lội", content = "Bơi lội giúp phát triển chiều cao", img = "boi.jpg" },
                new Blog { id_Blog = 4, id_TypeBlog = 5, tilte = "Ngủ đủ giấc", content = "Ngủ đủ giấc giúp cơ thể khỏe mạnh hon", img = "ngu.jpg" },
                new Blog { id_Blog = 5, id_TypeBlog = 3, tilte = "Tập thể dục", content = "Tập thể dục giúp cơ thể khỏe mạnh hơn", img = "theduc.jpg" },
                new Blog { id_Blog = 6, id_TypeBlog = 3, tilte = "Tập Yoga", content = "Tập Yoga giúp cơ thể dẻo dai hơn", img = "yoga.jpg" },
                new Blog { id_Blog = 7, id_TypeBlog = 2, tilte = "Corona", content ="Virut Corona làm suy giảm hệ miễn dịch", img = "corona.jpg"},
                new Blog { id_Blog = 8, id_TypeBlog = 2, tilte = "Bệnh tiểu đường", content = "Bệnh tiểu đường làm suy giảm hệ miễn dịch", img = "tieu.jpg" },
                new Blog { id_Blog = 9, id_TypeBlog = 2, tilte = "Bệnh tim mạch", content = "Bệnh tim mạch làm suy giảm hệ miễn dịch", img = "tim.jpg" },
                new Blog { id_Blog = 10, id_TypeBlog = 2, tilte = "Bệnh ung thư", content = "Bệnh ung thư làm suy giảm hệ miễn dịch", img = "ungthu.jpg" }

                );
            modelBuilder.Entity<Account>().HasData(
                new Account { id_Acc = 4, email = "nltkhanh3009@gmail.com", passWord = "Tkgame2603", role = "user" },
                new Account { id_Acc = 5, email = "felixfinn3009@gmail.com", passWord = "Tkg@me2603", role = "admin" }
                );
            modelBuilder.Entity<InfoUser>().HasData(
                new InfoUser { id_User = 1, id_Acc = 4, fullName = "Nguyễn Lê Trung Khánh", age = 22, gender = "Male", height = 176, weight = 84 }
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
