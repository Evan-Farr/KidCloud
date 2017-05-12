using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public string Status { get; set; }
        
        public virtual Parent Parent { get; set; }
        public virtual DayCare DayCare { get; set; }
    }
}