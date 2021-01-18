using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses.Treatments
{
    public class TreatmentModel
    {
        public long Id { get; set; }

        public bool IsExisting { get; set; }

        public long DiagnoseId { get; set; }

        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public DateTime End { get; set; }

        public DiagnosedSurgery Surgery { get; set; }

        public DiagnosedRadiation Radiation { get; set; }

        public DiagnosedChemeotherapy Chemeotherapy { get; set; }

        public DiagnosedEndocrineTreatment EndocrineTreatment { get; set; }
    }
}
