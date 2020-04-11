using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial6.DTOs.Requests;
using Tutorial6.DTOs.Responses;
using Tutorial6.Models;

namespace Tutorial6.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public Enrollment GetEnrollment(string indexNumber);
        public Enrollment GetEnrollment(int idEnrollment);
        public Student GetStudent(string indexNumber);
        public int DeleteStudnet(string indexNumber);
        public int InsertStudent(Student student);
        public int InsertOrUpdate(Student student);
        public StudentEnrollmentRes EnrollStudent(StudentEnrollmentReq req);
        public StudnetsPromotionRes PromoteStudnets(StudnetsPromotionReq req);
    }
}
