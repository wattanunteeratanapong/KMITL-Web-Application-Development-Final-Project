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
        public DbSet<Profile> Profiles { get; set; }

        // ✅ เพิ่มข้อมูลเริ่มต้นเข้าไป
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Profile>().HasData(
                new Profile
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Bio = "I love coding!",
                    Interests = "C#, .NET, MVC",
                    ProfilePictureUrl = "https://via.placeholder.com/150",
                    CoverPhotoUrl = "https://source.unsplash.com/800x300/?technology" // ✅ เพิ่มค่าเริ่มต้น
                },
                new Profile
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Bio = "Full-stack Developer",
                    Interests = "React, JavaScript, Node.js",
                    ProfilePictureUrl = "https://via.placeholder.com/150",
                    CoverPhotoUrl = "https://source.unsplash.com/800x300/?nature" // ✅ เพิ่มค่าเริ่มต้น
                }
            );
        }

    }
}
