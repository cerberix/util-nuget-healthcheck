using System.Threading.Tasks;

namespace Cerberix.Utility.HealthCheck
{
    public interface IHealthCheck
	{
		Task<HealthCheckResult> Run();
	}
}
