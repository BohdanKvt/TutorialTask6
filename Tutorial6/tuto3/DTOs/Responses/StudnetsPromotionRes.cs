using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial6.DTOs.Responses
{
    public class StudnetsPromotionRes
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
        public string Studies { get; set; }
        public string Error { get; set; }
    }
}
