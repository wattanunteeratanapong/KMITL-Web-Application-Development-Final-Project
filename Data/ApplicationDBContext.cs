using Microsoft.EntityFrameworkCore;
using Togeta.Models;

namespace Togeta.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
