using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public string title { get; set; }

        public Boolean allDay { get; set; }

        public string start { get; set; }

        public string end { get; set; }

        public Boolean overlap { get; set; }

    }
}