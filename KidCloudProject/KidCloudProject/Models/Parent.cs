﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class Parent
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
        [Required, DataType(DataType.PhoneNumber)]
        public PhoneAttribute Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int NumberOfChildren { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal? MoneyOwed { get; set; }


        public virtual DayCare DayCareId { get; set; }
        public virtual ApplicationUser UserId { get; set; }
    }
}