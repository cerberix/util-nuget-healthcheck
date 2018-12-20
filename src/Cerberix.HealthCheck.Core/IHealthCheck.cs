using System.Threading.Tasks;

namespace Cerberix.HealthCheck.Core
{
    public interface IHealthCheck
	{
		Task<HealthCheckResult> Run();
	}
}
