using System.Data.SqlClient;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace DataBaseConnectionExample
{
    public class DbRepository
    {
        private SqlConnection Connection { get; set; }
        public IConfiguration Configuration { get; set; }
        public DbRepository()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            Connection = new SqlConnection
            {
                ConnectionString = Configuration.GetConnectionString("ExampleDB")
            };
        }

        public string GetQuestions(string id)
        {
            using (Connection)
            {
                string dataFromDatabase = "";
                try
                {
                    Connection.Open();
                    string sqlQuery = $"SELECT * FROM [dbo].[questions];";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, Connection))
                    {
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dataFromDatabase = "FirstName: " + dataReader[0].ToString();
                                }
                            }
                            else
                            {
                                dataFromDatabase = "No data";
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    dataFromDatabase = "No Database Connection ! Error: \" + e.Message";
                    throw new Exception("No Database Connection ! Error: " + e.Message);
                }
                Connection.Close();
                return dataFromDatabase;
            }
        }

    }
}
