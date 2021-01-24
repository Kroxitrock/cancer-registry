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

        public async Task<Treatment> getByIdAsync(long id)
        {
            return await _diagnoseContext.Treatments
                .Where(treatment => treatment.Id == id)
                .Include(treatment => treatment.Diagnose)
                .SingleOrDefaultAsync();
        }

        public async Task<int> CreateAsync(Treatment treatment)
        {
            _diagnoseContext.Treatments.Add(treatment);
            return await _diagnoseContext.SaveChangesAsync();
        }
    }
}
