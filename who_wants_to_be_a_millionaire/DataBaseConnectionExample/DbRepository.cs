using System.Data.SqlClient;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        public List<List<string>> GetQuestions()
        {
            using (Connection)
            {
                List<List<string>> dataFromDatabase = new List<List<string>> ();
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
                                    List<string> currentRow = new List<string>()
                                    {
                                        ""+dataReader["question"].ToString(),
                                        ""+dataReader["a"].ToString(),
                                        ""+dataReader["b"].ToString(),
                                        ""+dataReader["c"].ToString(),
                                        ""+dataReader["d"].ToString(),
                                        ""+dataReader["correct"].ToString(),
                                        ""+dataReader["level"].ToString(),
                                    };
                                    dataFromDatabase.Add(currentRow);
                                }
                            }
                            else
                            {
                                dataFromDatabase.Add(new List<string> { "No data" });
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    dataFromDatabase.Add(new List<string> { "No Database Connection ! Error: \" + e.Message" });
                    throw new Exception("No Database Connection ! Error: " + e.Message);
                }
                Connection.Close();
                return dataFromDatabase;
            }
        }
    }
}
