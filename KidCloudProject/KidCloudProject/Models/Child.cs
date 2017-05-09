using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KidCloudProject.Models
{
    public class Child
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Age { get; set; }
        [Required]
        public bool HasAllergies { get; set; }
        [DataType(DataType.MultilineText)]
        public string AllergiesDetails { get; set; }
        [Required]
        public bool TakesMedications { get; set; }
        [DataType(DataType.MultilineText)]
        public string MedicationInstructions { get; set; }
        [Required]
        public bool HasSpecialFoodRequirements { get; set; }
        [DataType(DataType.MultilineText)]
        public string FoodRequirementsInstructions { get; set; }
        [Required]
        public bool IsSpecialNeeds { get; set; }
        public string SpecialNeedsType { get; set; }
        [DataType(DataType.MultilineText)]
        public string SpecialNeedsRequirements { get; set; }


        public virtual Parent ParentId { get; set; }
        public virtual ApplicationUser UserId { get; set; }
    }
}