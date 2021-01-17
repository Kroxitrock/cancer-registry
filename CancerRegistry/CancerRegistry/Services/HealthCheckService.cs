using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Models.Diagnoses.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class HealthCheckService
    {
        private readonly DiagnoseContext _diagnoseContext;

        public HealthCheckService(DiagnoseContext diagnoseContext)
        {
            _diagnoseContext = diagnoseContext;
        }

        public async Task<HealthCheck> getLastForDiagnoseAsync(long diagnoseId)
        {
            return await _diagnoseContext.HealthChecks
                .Where(check => check.Diagnose.Id == diagnoseId)
                .OrderByDescending(check => check.Timestamp)
                .FirstOrDefaultAsync();
        }

        public async Task<HealthCheck> GetByIdAsync(long id)
        {
            return await _diagnoseContext.HealthChecks
                .Where(healthCheck => healthCheck.Id == id)
                .Include(healthCheck => healthCheck.Diagnose)
                .SingleOrDefaultAsync();
        }

        public async Task<int> CreateAsync(HealthCheck healthCheck)
        {
            await _diagnoseContext.HealthChecks.AddAsync(healthCheck);
            return await _diagnoseContext.SaveChangesAsync();
        }
    }
}
