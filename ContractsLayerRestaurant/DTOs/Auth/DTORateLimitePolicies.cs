using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTOs.Auth
{
    public class DTORateLimitePolicies
    {
        public RateLimiterOptions options { get; set; }
        public string NamePolicy { get; set; }
        public int PermitLimit { get; set; }
        public int TimeSpam { get; set; }
        public int QueueLimit { get; set; } = 0;

        public DTORateLimitePolicies(RateLimiterOptions options, string NamePolicy, int PermitLimit, int TimeSpam, int QueueLimit)
        {
            this.options = options;
            this.PermitLimit = PermitLimit;
            this.NamePolicy = NamePolicy;
            this.QueueLimit = QueueLimit;
            this.TimeSpam = TimeSpam;
        }
    }
}
