namespace Cerberix.HealthCheck.Core
{
    public class HealthCheckResult
    {
        public string Name { get; }
        public string Description { get; }
        public string Error { get; }
        public string Status { get; }

        public HealthCheckResult(
            string name,
            string description,
            string error,
            string status
            )
        {
            Name = name;
            Description = description;
            Error = error;
            Status = status;
        }
    }
}
