using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;

using System.IO;

namespace Tutorial6.Services
{
    public class SqlServerDbService : IDbService
    {
        public bool ExistIndexNumber(string index)
        {
            using (var sqlConnection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s16563;Integrated Security=True"))
            {
                sqlConnection.Open();

                using (var command = new SqlCommand())
                {

                    command.Connection = sqlConnection;
                    command.CommandText = "select count(1) as 'count' from Student where IndexNumber = @IndexNum;";
                    command.Parameters.AddWithValue("IndexNum", index);
                    var response = command.ExecuteReader();
                    response.Read();
                    if (response["count"].ToString().Equals("1"))
                    {

                        response.Close();
                        sqlConnection.Close();
                        return true;
                    }

                    response.Close();
                }
                sqlConnection.Close();
            }

            return false;
        }

        public void SaveLogData(string data)
        {

            using var sw = File.AppendText(@"requestsLog.txt");
            sw.WriteLine("Request start:");
            sw.WriteLine(data);
            sw.WriteLine("Request end: ");
        }


    }
}