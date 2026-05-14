
namespace ContractsLayerRestaurant.DTORequest.Auth
{
    public class RateLimitePoliciesOptions
    {
        public string NamePolicy { get; set; }
        public int PermitLimit { get; set; }
        public int TimeSpan { get; set; }
        public int QueueLimit { get; set; } = 0;

        public RateLimitePoliciesOptions(string NamePolicy, int PermitLimit, int TimeSpan, int QueueLimit)
        {
            this.PermitLimit = PermitLimit;
            this.NamePolicy = NamePolicy;
            this.QueueLimit = QueueLimit;
            this.TimeSpan = TimeSpan;
        }
    }
}
