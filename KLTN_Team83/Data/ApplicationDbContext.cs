using KLTN_Team83.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace KLTN_Team83.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions <ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
