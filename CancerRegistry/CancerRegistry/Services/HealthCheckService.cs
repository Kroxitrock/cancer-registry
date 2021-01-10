using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Models.Diagnoses.HealthChecks;
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

        public HealthCheck getLastForDiagnose(long diagnoseId)
        {
            return _diagnoseContext.HealthChecks
                .Where(check => check.Diagnose.Id == diagnoseId)
                .OrderByDescending(check => check.Timestamp)
                .FirstOrDefault();
        }
    }
}
