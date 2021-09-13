using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrInterviews.Models
{
    public class InterviewViewModel
    {
        public int InterviewId { get; set; }

        [MaxLength(4000)]
        public string Feedback { get; set; }

        public DateTime InterviewDate { get; set; }

        public List<SelectListItem> AvailableProfiles { get; set; } = new();

        public int SelectedProfileId { get; set; }

        public ProfileViewModel Profile { get; set; } = new();
    }
}
