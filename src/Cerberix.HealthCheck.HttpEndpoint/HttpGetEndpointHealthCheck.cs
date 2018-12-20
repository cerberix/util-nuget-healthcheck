using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cerberix.HealthCheck.Core;

namespace Cerberix.HealthCheck.HttpEndpoint
{
	public class HttpGetEndpointHealthCheck : IHealthCheck
	{
		private readonly string _description;
        private readonly string _endpoint;
        private readonly TimeSpan _connectTimout;

        public HttpGetEndpointHealthCheck(
			string description,
            string endpoint,
            int connectTimeout
            )
        {
			_description = description;
            _endpoint = endpoint;
            _connectTimout = new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: connectTimeout, milliseconds: 0);
		}

        public async Task<HealthCheckResult> Run()
        {
            HealthCheckStatus status;
            string error;

            try
            {
                bool hasResult = false;

                using (var client = new HttpClient() { Timeout = _connectTimout })
                {
                    var response = await client.GetAsync(_endpoint, HttpCompletionOption.ResponseHeadersRead);

                    hasResult = response.IsSuccessStatusCode;
                }

                status = hasResult ? HealthCheckStatus.Pass : HealthCheckStatus.Fail;
                error = null;
            }
            catch (Exception exception)
            {
                status = HealthCheckStatus.Fail;
                error = exception.Message;
            }

            return new HealthCheckResult(nameof(HttpGetEndpointHealthCheck), _description, error, status.ToString());
        }
    }
}
