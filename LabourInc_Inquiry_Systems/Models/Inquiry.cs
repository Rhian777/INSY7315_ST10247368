using System.ComponentModel.DataAnnotations;

namespace LabourInc_Inquiry_Systems.Models
{
    public class Inquiry
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Company { get; set; } = string.Empty;

        [Required, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string ServiceRequested { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public string? AttachmentFileName { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "New";

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
    

}
