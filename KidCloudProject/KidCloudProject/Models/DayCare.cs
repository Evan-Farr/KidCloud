using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class DayCare
    {
        public DayCare()
        {
            DailyReports = new List<DailyReport>();
            Parents = new List<Parent>();
            Employees = new List<Employee>();
            Children = new List<Child>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime? OpenDate { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Required, StringLength(5)]
        public string ZipCode { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public PhoneAttribute Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool AcceptChildrenUnderAgeTwo { get; set; }
        [Required]
        public bool AcceptDisabilites { get; set; }
        [Required]
        public int MaxChildren { get; set; }
        public bool CurrentlyAcceptingApplicants { get; set; }


        public virtual ICollection<DailyReport> DailyReports { get; set; }
        public virtual ICollection<Parent> Parents { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Child> Children { get; set; }
        public virtual ApplicationUser UserId { get; set; }
    }
}