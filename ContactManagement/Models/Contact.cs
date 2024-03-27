using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactManagement.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(16, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 16 characters.")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(12, MinimumLength = 5, ErrorMessage = "Number must be between 5 and 12 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Number must be digits only.")]
        public string Number { get; set; }
        
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}