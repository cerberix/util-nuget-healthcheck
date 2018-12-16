using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cerberix.Utility.HealthCheck
{
	internal class HttpGetEndpointHealthCheck : IHealthCheck
	{
		private readonly string _name;
		private readonly string _description;
        private readonly string _endpoint;
        private readonly TimeSpan _timeout;

        public HttpGetEndpointHealthCheck(
			string name,
			string description,
            string endpoint,
            int timeout
            )
        {
			_name = name;
			_description = description;
            _endpoint = endpoint;
            _timeout = new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: timeout, milliseconds: 0);
		}

        public async Task<HealthCheckResult> Run()
        {
            HealthCheckStatus status;
            string error;

            try
            {
                bool hasResult = false;

                using (var client = new HttpClient() { Timeout = _timeout })
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

            return new HealthCheckResult(_name, _description, error, status);
        }
    }
}
