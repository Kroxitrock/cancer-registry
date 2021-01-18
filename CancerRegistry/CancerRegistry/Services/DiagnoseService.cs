using CancerRegistry.Models.Diagnoses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class DiagnoseService
    {
        private readonly DiagnoseContext _diagnoseContext;

        public DiagnoseService(DiagnoseContext diagnoseContext)
        {
            _diagnoseContext = diagnoseContext;
        }

        public async Task<Diagnose> GetByIdAsync(long id)
        {
            return await _diagnoseContext.Diagnoses
                .Include(d => d.Patient)
                .Include(d => d.Treatment)
                .Where(d => d.Id == id)
                .SingleOrDefaultAsync();
        }

        /**
         * A method that allows us to determine the stage of the breast cancer. Stages are computed based on the rules given by Digital Health Assistant.
         */
        internal short DetermineStage(DistantMetastasisState distantMetastasis, PrimaryTumorState primaryTumor, RegionalLymphNodesState regionalLymphNodes)
        {

            // Stage 4 cancer
            if (distantMetastasis == DistantMetastasisState.M1)
            {
                return 4;
            }

            // Stage 0 cancer
            if (regionalLymphNodes == RegionalLymphNodesState.N0 && primaryTumor == PrimaryTumorState.T0)
            {
                return 0;
            }
            
            // Stage 1 cancer
            if ((regionalLymphNodes == RegionalLymphNodesState.N1 && primaryTumor == PrimaryTumorState.T0)
                  || (regionalLymphNodes == RegionalLymphNodesState.N1 && primaryTumor == PrimaryTumorState.T1)
                  || (regionalLymphNodes == RegionalLymphNodesState.N1 && primaryTumor == PrimaryTumorState.T1))
            {
                return 1;
            }
            
            // Stage 2 cancer
            if ((regionalLymphNodes == RegionalLymphNodesState.N1 && primaryTumor == PrimaryTumorState.T1)
                  || (regionalLymphNodes == RegionalLymphNodesState.N0 && primaryTumor == PrimaryTumorState.T2)
                  || (regionalLymphNodes == RegionalLymphNodesState.N1 && primaryTumor == PrimaryTumorState.T2)
                  || (regionalLymphNodes == RegionalLymphNodesState.N0 && primaryTumor == PrimaryTumorState.T3))
            {
                return 2;
            }

            // Everything inbetween falls into the Stage 3 category
            return 3;
        }

        internal async Task updateAsync()
        {
            await _diagnoseContext.SaveChangesAsync();
        }
    }
}
