using System;
using System.ComponentModel.DataAnnotations;

namespace EventSourcingMedium.API.Models
{
    public class PostInformation
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
