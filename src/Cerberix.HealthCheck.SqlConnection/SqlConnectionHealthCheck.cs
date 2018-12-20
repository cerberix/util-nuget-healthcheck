using System;
using System.Threading.Tasks;
using Cerberix.HealthCheck.Core;

namespace Cerberix.HealthCheck.SqlConnection
{
	public class SqlConnectionHealthCheck : IHealthCheck
	{
		private readonly string _description;
        private readonly string _connectionString;
        private readonly int _connectTimeout;

		public SqlConnectionHealthCheck(
			string description,
            string connectionString,
            int connectTimeout
			)
		{
			_description = description;
            _connectionString = connectionString;
            _connectTimeout = connectTimeout;
		}

		public async Task<HealthCheckResult> Run()
		{
			HealthCheckStatus status;
            string error;

			try
			{
                bool hasResult = false;

                using (var conn = new System.Data.SqlClient.SqlConnection(_connectionString))
                {
                    using (var cmd = new System.Data.SqlClient.SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT SYSDATETIME();";
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandTimeout = _connectTimeout;

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

            return new HealthCheckResult(nameof(SqlConnectionHealthCheck), _description, error, status.ToString());
		}
	}
}
