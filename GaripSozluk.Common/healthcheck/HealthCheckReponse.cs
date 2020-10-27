using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Common.healthcheck
{
  public  class HealthCheckReponse
    {
        public string Status { get; set; }
        public IndividualHealthCheckResponse Checks { get; set; }
        public TimeSpan HealthCheckDuration { get; set; }
    }
}
