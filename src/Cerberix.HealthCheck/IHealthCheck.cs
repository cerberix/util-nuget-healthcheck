using System.Threading.Tasks;

namespace Cerberix.HealthCheck
{
    public interface IHealthCheck
	{
		Task<HealthCheckResult> Run();
	}
}
