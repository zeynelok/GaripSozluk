using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Common.healthcheck
{
   public class IndividualHealthCheckResponse
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
    }
}
