using System;
using System.Threading.Tasks;
using Cerberix.HealthCheck.Core;

namespace Cerberix.HealthCheck.MySqlConnection
{
	public class MySqlConnectionHealthCheck : IHealthCheck
	{
		private readonly string _description;
        private readonly string _connectionString;
        private readonly int _connectTimeout;

		public MySqlConnectionHealthCheck(
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

                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
                {
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT NOW();";
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

            return new HealthCheckResult(nameof(MySqlConnectionHealthCheck), _description, error, status.ToString());
		}
	}
}
