using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class DirectMessageChannel
    {
        [Key]
        public int Id { get; set; }
        public string ChannelId { get; set; }

        public virtual ApplicationUser SenderId { get; set; }
        public virtual ApplicationUser ReciverId { get; set; }
    }
}