using System.ComponentModel.DataAnnotations;

namespace LabourInc_Inquiry_Systems.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Role { get; set; } = "Admin";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    

}
