using System.ComponentModel.DataAnnotations;

namespace Togeta.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Gender { get; set; }
        public int CommentCount { get; set; }


        public string? RequestId { get; set; }
        public bool ShowRequestId { get; set; } = false;

        // ✅ เพิ่มฟิลด์สำหรับโปรไฟล์
        public string Bio { get; set; } = string.Empty;
        public float CreditScore { get; set; }
        public string Interests { get; set; } = "";
        public string ProfileImagePath { get; set; } = "/default-profile.jpg";
        public string CoverImagePath { get; set; } = "/default-cover.jpg";


        // ✅ ฟิลด์ FullName สำหรับแสดงผลใน View
        public string FullName => $"{FirstName} {LastName}";


    }
}
