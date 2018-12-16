using System;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Cerberix.Utility.HealthCheck
{
	public class SqlConnectionHealthCheck : IHealthCheck
	{
		private readonly string _name;
		private readonly string _description;
        private readonly string _connectionString;
        private readonly int _connectionTimeout;

		public SqlConnectionHealthCheck(
			string name,
			string description,
            string connectionString,
            int connectionTimeout
			)
		{
			_name = name;
			_description = description;
            _connectionString = connectionString;
            _connectionTimeout = connectionTimeout;
		}

		public async Task<HealthCheckResult> Run()
		{
			HealthCheckStatus status;
            string error;

			try
			{
                bool hasResult = false;

                using (var conn = new SqlConnection())
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT SYSDATETIME();";
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = _connectionTimeout;

                        await conn.OpenAsync();

                        var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            hasResult |= true;
                        }

                        conn.Close();
                    }
                }

                status = hasResult ? HealthCheckStatus.Pass : HealthCheckStatus.Fail;
                error = null;
            }
			catch (Exception exception)
			{
				status = HealthCheckStatus.Fail;
                error = exception.Message;               
            }

            return new HealthCheckResult(_name, _description, error, status);
		}
	}
}
