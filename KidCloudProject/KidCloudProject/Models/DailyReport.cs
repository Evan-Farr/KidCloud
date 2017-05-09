using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class DailyReport
    {
        [Key]
        public int Id { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime ReportDate { get; set; }
        [Required, DataType(DataType.MultilineText)]
        public string BathroomUse { get; set; }
        [DataType(DataType.MultilineText)]
        public string Meals { get; set; }
        [DataType(DataType.MultilineText)]
        public string Sleep { get; set; }
        [DataType(DataType.MultilineText)]
        public string ActivityReport { get; set; }
        [DataType(DataType.MultilineText)]
        public string SuppliesNeeds { get; set; }
        [DataType(DataType.MultilineText)]
        public string Mood { get; set; }
        [DataType(DataType.MultilineText)]
        public string MiscellaneousNotes { get; set; }

        public virtual Child ChildId { get; set; }
    }
}