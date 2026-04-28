using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractsLayerRestaurant.DTORequest.Auth
{
    public class RateLimitePoliciesOptions
    {
        public string NamePolicy { get; set; }
        public int PermitLimit { get; set; }
        public int TimeSpam { get; set; }
        public int QueueLimit { get; set; } = 0;

        public RateLimitePoliciesOptions(string NamePolicy, int PermitLimit, int TimeSpam, int QueueLimit)
        {
            this.PermitLimit = PermitLimit;
            this.NamePolicy = NamePolicy;
            this.QueueLimit = QueueLimit;
            this.TimeSpam = TimeSpam;
        }
    }
}
