namespace Cerberix.Utility.HealthCheck
{
    public class HealthCheckResult
    {
        public string Name { get; }
        public string Description { get; }
        public string Error { get; }
        public HealthCheckStatus Status { get; }

        public HealthCheckResult(
            string name,
            string description,
            string error,
            HealthCheckStatus status
            )
        {
            Name = name;
            Description = description;
            Error = error;
            Status = status;
        }
    }
}
