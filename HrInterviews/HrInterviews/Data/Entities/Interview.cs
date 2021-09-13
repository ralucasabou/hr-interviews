using HrInterviews.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrInterviews.Data.Entities
{
    public class Interview
    {
        [Key]
        public int InterviewId { get; set; }

        [Required]
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }

        public DateTime InterviewDate { get; set; }

        [MaxLength(4000)]
        public string Feedback { get; set; }
    }
}
