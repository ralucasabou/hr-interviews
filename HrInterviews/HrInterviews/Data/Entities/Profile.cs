using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrInterviews.Data.Entities
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(400)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Studies { get; set; } = string.Empty;

        
        [MaxLength(4000)]
        public string PastExperiene { get; set; } = string.Empty;

        
        [MaxLength(4000)]
        public string Skills { get; set; } = string.Empty;

   
      
        public bool DrivingLicense { get; set; } = false;
    }
}
