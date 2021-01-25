using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses.Treatments
{
    public class CurrentTreatmentOutputModel
    {
        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string Treatment { get; set; }
        
        public string Description { get; set; }
        
        public string AddedOn { get; set; }
    }
}
