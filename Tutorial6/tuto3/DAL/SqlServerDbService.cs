﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tutorial6.DTOs.Requests;
using Tutorial6.DTOs.Responses;
using Tutorial6.Models;

namespace Tutorial6.DAL
{
    public class SqlServerDbService : IDbService
    {
        private const string ConnectionString = "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s18885;Integrated Security=True";

        public int DeleteStudnet(string indexNumber)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "delete from student where indexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public Enrollment GetEnrollment(string indexNumber)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select st.name as studies, semester, startDate " +
                                    "from Student s " +
                                    "join Enrollment e " +
                                    "on s.IdEnrollment = e.IdEnrollment " +
                                    "join Studies st " +
                                    "on e.idstudy = st.IdStudy " +
                                    "where s.indexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                Enrollment student = new Enrollment
                {
                    StartDate = DateTime.Parse(dataReader["startDate"].ToString()),
                    Studies = dataReader["studies"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString())
                };
                return student;
            }

            return null;
        }

        public Student GetStudent(string indexNumber)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select indexNumber, firstName, LastName, birthDate, st.name as studies, semester " +
                                    "from Student s " +
                                    "join Enrollment e " +
                                    "on s.IdEnrollment = e.IdEnrollment " +
                                    "join Studies st " +
                                    "on e.idstudy = st.IdStudy " +
                                    "where s.indexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                Student student = new Student
                {
                    IndexNumber = dataReader["indexNumber"].ToString(),
                    FirstName = dataReader["firstName"].ToString(),
                    LastName = dataReader["lastName"].ToString(),
                    BirthDate = DateTime.Parse(dataReader["birthDate"].ToString()),
                    Studies = dataReader["studies"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString())
                };
                return student;
            }

            return null;
        }

        public IEnumerable<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select indexNumber, firstName, LastName, birthDate, st.name as studies, semester " +
                                    "from Student s " +
                                    "join Enrollment e " +
                                    "on s.IdEnrollment = e.IdEnrollment " +
                                    "join Studies st " +
                                    "on e.idstudy = st.IdStudy "
            };

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                Student student = new Student
                {
                    IndexNumber = dataReader["indexNumber"].ToString(),
                    FirstName = dataReader["firstName"].ToString(),
                    LastName = dataReader["lastName"].ToString(),
                    BirthDate = DateTime.Parse(dataReader["birthDate"].ToString()),
                    Studies = dataReader["studies"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString())
                };
                students.Add(student);
            }

            return students;
        }

        public int InsertOrUpdate(Student student)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "update student set firstName=@firstName, lastName=@lastName, birthDate=@birthDate, idEnrollment=@idEnrollment " +
                                "where indexNumber=@indexNumber " +
                                @"if @@rowcount=0 " +
                                "insert into student (indexNumber, firstName, lastName, birthDate, idEnrollment) " +
                                "values(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)"
            };
            command.Parameters.AddWithValue("indexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate.ToString());
            command.Parameters.AddWithValue("idEnrollment", student.Semester);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int InsertStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "insert into student (indexNumber, firstName, lastName, birthDate, idEnrollment) " +
                                    "values(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)"
            };
            command.Parameters.AddWithValue("indexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate.ToString());
            command.Parameters.AddWithValue("idEnrollment", student.Semester);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public StudentEnrollmentRes EnrollStudent(StudentEnrollmentReq request)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM Studies WHERE Name=@Name"
            };
            command.Parameters.AddWithValue("Name", request.Studies);

            connection.Open();

            var transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            int idEnrollment;

            try
            {
                var dr = command.ExecuteReader();
                // Check if the Study exists
                if (!dr.Read())
                {
                    dr.Close();
                    transaction.Rollback();
                    return new StudentEnrollmentRes
                    {
                        Error = "Study does not exist"
                    };
                }
                int idStudy = (int)dr["iDstudy"];

                command.CommandText = "Select * from Student where IndexNumber=@IndexNumber";
                command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                dr.Close();
                dr = command.ExecuteReader();
                // Check student with given index exists
                if (dr.Read())
                {
                    dr.Close();
                    transaction.Rollback();
                    return new StudentEnrollmentRes
                    {
                        Error = "Student with given index exists"
                    };
                }

                command.CommandText = "SELECT * FROM Enrollment WHERE Semester=1 AND IdStudy=@IdStudy";
                command.Parameters.AddWithValue("IdStudy", idStudy);
                dr.Close();
                dr = command.ExecuteReader();

                if (dr.Read())
                {
                    idEnrollment = (int)dr["IdEnrollment"];
                    dr.Close();
                }
                // If there is no Enrollment with a given Study add new Enrollment
                else
                {
                    command.CommandText = "SELECT MAX(idEnrollment) as IdEnrollment FROM Enrollment";
                    dr.Close();
                    dr = command.ExecuteReader();
                    var nextIdEnrollment = 1;
                    if (dr.Read())
                    {
                        nextIdEnrollment = (int)dr["IdEnrollment"] + 1;
                    }
                    dr.Close();
                    var startDate = DateTime.Now;
                    command.CommandText = "INSERT INTO Enrollment(idEnrollment, idStudy, semester, startDate) " +
                                                          "values(@NextIdEnrollment, @IdStudy, 1, @StartDate)";
                    command.Parameters.AddWithValue("NextIdEnrollment", nextIdEnrollment);
                    command.Parameters.AddWithValue("StartDate", startDate);
                    command.ExecuteNonQuery();
                    idEnrollment = nextIdEnrollment;
                }

                command.CommandText = "INSERT INTO Student(IndexNumber, firstname, lastname, birthdate, idenrollment) " +
                                                            "values(@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";
                command.Parameters.AddWithValue("FirstName", request.FirstName);
                command.Parameters.AddWithValue("LastName", request.LastName);
                command.Parameters.AddWithValue("BirthDate", request.BirthDate);
                command.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (SqlException)
            {
                transaction.Rollback();
                return new StudentEnrollmentRes
                {
                    Error = "SQLException"
                };
            }
            return new StudentEnrollmentRes
            {
                IdEnrollment = idEnrollment,
                Semester = 1
            };
        }

        public Enrollment GetEnrollment(int idEnrollment)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand
            {
                Connection = connection,
                CommandText = "select idenrollment, name, semester, startdate " +
                                       "from enrollment e join studies s " +
                                       "on e.idstudy = s.idstudy " +
                                       "where e.idenrollment = @IdEnrollment"
            };
            command.Parameters.AddWithValue("IdEnrollment", idEnrollment);

            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                Enrollment enrollment = new Enrollment
                {
                    IdEnrollment = int.Parse(dataReader["idenrollment"].ToString()),
                    Studies = dataReader["name"].ToString(),
                    Semester = int.Parse(dataReader["semester"].ToString()),
                    StartDate = DateTime.Parse(dataReader["startdate"].ToString())
                };
                return enrollment;
            }

            return null;
        }

        public StudnetsPromotionRes PromoteStudnets(StudnetsPromotionReq req)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            using SqlCommand command = new SqlCommand("PromoteStudents")
            {
                Connection = connection,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@StudiesName", req.Studies));
            command.Parameters.Add(new SqlParameter("@Semester", req.Semester));

            connection.Open();
            try
            {
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    StudnetsPromotionRes enrollment = new StudnetsPromotionRes
                    {
                        IdEnrollment = int.Parse(dataReader["idenrollment"].ToString()),
                        Studies = dataReader["name"].ToString(),
                        Semester = int.Parse(dataReader["semester"].ToString()),
                        StartDate = DateTime.Parse(dataReader["startdate"].ToString())
                    };
                    return enrollment;
                }
            }
            catch (SqlException e)
            {
                return new StudnetsPromotionRes { Error = e.Message};
            }
            return null;
        }
    }
}
