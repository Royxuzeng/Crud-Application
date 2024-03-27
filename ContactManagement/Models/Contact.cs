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
        public string Name { get; set; }
        [DisplayName("Number")]
        [Required]
        public string Number { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}