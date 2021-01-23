using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses
{
    public class CurrentDiagnoseOutputModel
    {
        public string PatientName { get; set; }
        
        public string DoctorName { get; set; }

        public string Stage { get; set; }
        
        public string PrimaryTumorState { get; set; }
        public string DistantMetastasisState { get; set; }
        public string RegionalLymphNodesState { get; set; }
    }
}
