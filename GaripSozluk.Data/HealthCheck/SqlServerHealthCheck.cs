using Abp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GaripSozluk.Data.HealthCheck
{
    public class SqlServerHealthCheck : IHealthCheck
    {
        private readonly GaripSozlukDbContext _dbContext;
        public SqlServerHealthCheck(GaripSozlukDbContext dbContext)
        
        {
            _dbContext = dbContext;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
               
                    if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                    {
                        return HealthCheckResult.Healthy("could connect to database");
                    }
                
                return HealthCheckResult.Unhealthy("could not connect to database");
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy("Error when trying to check HealthCheckExampleDbContext. ", e);
            }
        }
    }
}
