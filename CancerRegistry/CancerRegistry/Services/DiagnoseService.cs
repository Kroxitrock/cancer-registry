using CancerRegistry.Models.Diagnoses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CancerRegistry.Services
{
    public class DiagnoseService
    {
        private readonly DiagnoseContext _diagnoseContext;

        public DiagnoseService(DiagnoseContext diagnoseContext)
        {
            _diagnoseContext = diagnoseContext;
        }

        public Diagnose GetById(long id)
        {
            return _diagnoseContext.Diagnoses
                .Where(d => d.Id == id)
                .SingleOrDefault();
        }
    }
}
