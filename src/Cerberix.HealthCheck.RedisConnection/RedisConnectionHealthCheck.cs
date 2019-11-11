using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Cerberix.HealthCheck.RedisConnection
{
    public class RedisConnectionHealthCheck : IHealthCheck
	{
        private static Lazy<ConnectionMultiplexer> _connection;

		private readonly string _description;

		public RedisConnectionHealthCheck(
			string description,
            string connectionString,
            int connectTimeout,
            int connectRetry
			)
		{
			_description = description;

            {
                var options = GetConfigurationOptions(connectionString, connectTimeout, connectRetry);

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

            return new HealthCheckResult(nameof(RedisConnectionHealthCheck), _description, error, status.ToString());
		}

        private static ConfigurationOptions GetConfigurationOptions(string connectionString, int connectTimeout, int connectRetry)
        {
            var options = ConfigurationOptions.Parse(connectionString);

            options.ConnectTimeout = 1000 * connectTimeout; //  convert seconds => milliseconds
            options.ConnectRetry = connectTimeout;

            if (options.AbortOnConnectFail)
            {
                options.AbortOnConnectFail = false;
            }
            
            return options;
        }
	}
}
