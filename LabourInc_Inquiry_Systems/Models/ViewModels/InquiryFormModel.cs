using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LabourInc_Inquiry_Systems.Models.ViewModels
{
    public class InquiryFormViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string ServiceRequested { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public IFormFile? Attachment { get; set; }
    }
    

}
