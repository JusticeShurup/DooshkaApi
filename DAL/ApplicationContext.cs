using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users{ get; set; }

        public DbSet<ToDoItem> ToDoItems { get; set; }

        public DbSet<RevokedToken> RevokedTokens { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options) {}
    }
}
