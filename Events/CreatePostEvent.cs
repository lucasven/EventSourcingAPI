using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class CreatePostEvent : Event
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
