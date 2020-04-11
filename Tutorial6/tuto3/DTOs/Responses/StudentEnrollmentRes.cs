using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial6.DTOs.Responses
{
    public class StudentEnrollmentRes
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public string Error { get; set; }
    }
}
