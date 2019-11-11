using System.Linq;
using System.Threading.Tasks;

namespace Cerberix.HealthCheck
{
	public class HealthCheckService
	{
		private readonly IHealthCheck[] _healthChecks;

		public HealthCheckService(
			IHealthCheck[] healthChecks
			)
		{
			_healthChecks = healthChecks;
		}

		public async Task<HealthCheckResult[]> GetResults()
		{
			var results = await Task.WhenAll(_healthChecks.Select(async hc => await hc.Run()));
			return results.ToArray();
		}
	}
}
