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

        public string? RequestId { get; set; }
        public bool ShowRequestId { get; set; } = false; 
    }
}
