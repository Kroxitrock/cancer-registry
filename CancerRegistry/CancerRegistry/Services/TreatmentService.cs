using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Models.Diagnoses.Treatments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class TreatmentService
    {
        private readonly DiagnoseContext _diagnoseContext;

        public TreatmentService(DiagnoseContext diagnoseContext)
        {
            _diagnoseContext = diagnoseContext;
        }

        public async Task<Treatment> GetByIdAsync(long id)
        {
            return await _diagnoseContext.Treatments
                .Where(treatment => treatment.Id == id)
                .Include(treatment => treatment.Diagnose)
                .SingleOrDefaultAsync();
        }

        public async Task<long> CreateAsync(Treatment treatment)
        {
             await _diagnoseContext.Treatments.AddAsync(treatment);
             await _diagnoseContext.SaveChangesAsync();

             return treatment.Id;
        }
    }
}
