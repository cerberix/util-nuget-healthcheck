using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Cerberix.Utility.HealthCheck
{
    public class RedisConnectionHealthCheck : IHealthCheck
	{
        private static Lazy<ConnectionMultiplexer> _connection;

        private readonly string _name;
		private readonly string _description;

		public RedisConnectionHealthCheck(
			string name,
			string description,
            string connectionString,
            int connectionTimeout
			)
		{
			_name = name;
			_description = description;

            {
                var options = ConfigurationOptions.Parse(connectionString);
                options.ConnectTimeout = connectionTimeout;

                _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
            }
        }

		public async Task<HealthCheckResult> Run()
		{
			HealthCheckStatus status;
            string error;

			try
			{
                bool hasResult = await Task.FromResult(_connection.Value.IsConnected);
              
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
