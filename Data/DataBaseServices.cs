using System.Data;
using System;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SagErpBlazor.Data
{
    public class DataBaseServices
    {
        private readonly string connectionString;

        public DataBaseServices(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnectionStringName");
        }

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connectionString);
        }

        public DataTable GetConfigurationData(string whereClause = "")
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("STS_SystemConfiguration_SelectAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.Parameters.Add(new SqlParameter("@WhereClause", whereClause));
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return dt;
        }

    }


}
