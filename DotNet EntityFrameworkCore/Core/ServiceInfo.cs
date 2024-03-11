namespace DotNet_EntityFrameworkCore.Core
{
    public class ServiceInfo
    {
        public ServiceInfo(string serviceCode, string serviceName, string description = null, string environment = null)
        {
            ServiceCode = serviceCode;
            ServiceName = serviceName;
            Description = description;
            Environment = environment;

        }
        public string ServiceCode { get; }
        public string ServiceName { get; }
        public string Description { get; }
        public string Environment { get; }
    }
}
