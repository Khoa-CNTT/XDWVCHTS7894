using KLTN_Team83.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Net.Sockets;

namespace KLTN_Team83.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext( DbContextOptions <ApplicationDbContext> options) : base(options) { }

        public DbSet<TypeBlog> TypeBlogs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<GoalType> GoalTypes { get; set; }
        //mục tiêu
        public DbSet<Goal> Goals { get; set; }
        //thói quen
        public DbSet<Habit> Habits { get; set; }
        //Nhật ký thực hiện thói quen
        public DbSet<HabitLog> HabitLogs { get; set; }
        //thời gian biểu
        public DbSet<ScheduleItem> ScheduleItems { get; set; }

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
                new Blog { id_Blog = 1, id_TypeBlog = 1, tilte = "Rau củ", content = "Rau củ quả tốt cho hệ tiêu hóa", ImageUrl = "raucu.jpg" },
                new Blog { id_Blog = 2, id_TypeBlog = 1, tilte = "Uống nước", content = "Uống nước giúp cơ thể lưu thông máu tốt hơn", ImageUrl = "nuoc.jpg" },
                new Blog { id_Blog = 3, id_TypeBlog = 3, tilte = "Bơi lội", content = "Bơi lội giúp phát triển chiều cao", ImageUrl = "boi.jpg" },
                new Blog { id_Blog = 4, id_TypeBlog = 5, tilte = "Ngủ đủ giấc", content = "Ngủ đủ giấc giúp cơ thể khỏe mạnh hon", ImageUrl = "ngu.jpg" },
                new Blog { id_Blog = 5, id_TypeBlog = 3, tilte = "Tập thể dục", content = "Tập thể dục giúp cơ thể khỏe mạnh hơn", ImageUrl = "theduc.jpg" },
                new Blog { id_Blog = 6, id_TypeBlog = 3, tilte = "Tập Yoga", content = "Tập Yoga giúp cơ thể dẻo dai hơn", ImageUrl = "yoga.jpg" },
                new Blog { id_Blog = 7, id_TypeBlog = 2, tilte = "Corona", content ="Virut Corona làm suy giảm hệ miễn dịch", ImageUrl = "corona.jpg"},
                new Blog { id_Blog = 8, id_TypeBlog = 2, tilte = "Bệnh tiểu đường", content = "Bệnh tiểu đường làm suy giảm hệ miễn dịch", ImageUrl = "tieu.jpg" },
                new Blog { id_Blog = 9, id_TypeBlog = 2, tilte = "Bệnh tim mạch", content = "Bệnh tim mạch làm suy giảm hệ miễn dịch", ImageUrl = "tim.jpg" },
                new Blog { id_Blog = 10, id_TypeBlog = 2, tilte = "Bệnh ung thư", content = "Bệnh ung thư làm suy giảm hệ miễn dịch", ImageUrl = "ungthu.jpg" }

                );
            modelBuilder.Entity<GoalType>().HasData(
                new GoalType { Id_GoalType = 1, NameGoalType="Weight" },
                new GoalType { Id_GoalType=2, NameGoalType="Height"},
                new GoalType { Id_GoalType = 3, NameGoalType = "Sleep" }
                );

            var daysOfWeekConverter = new ValueConverter<List<DayOfWeek>, string>(
        v => string.Join(',', v),
        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(d => Enum.Parse<DayOfWeek>(d)).ToList());

            var daysOfWeekComparer = new ValueComparer<List<DayOfWeek>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            modelBuilder.Entity<Habit>()
                .Property(h => h.DaysOfWeek)
                .HasConversion(daysOfWeekConverter)
                .Metadata.SetValueComparer(daysOfWeekComparer);
            modelBuilder.Entity<HabitLog>().HasOne(h => h.Habit).WithMany(h => h.HabitLogs).HasForeignKey(h => h.Id_Habit).OnDelete(DeleteBehavior.Restrict); // hoặc .NoAction
        }

    }
}
