using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int? Age { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Required, StringLength(5)]
        public string ZipCode { get; set; }
        [Required, StringLength(10)]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }


        public virtual ApplicationUser UserId { get; set; }
    }
}