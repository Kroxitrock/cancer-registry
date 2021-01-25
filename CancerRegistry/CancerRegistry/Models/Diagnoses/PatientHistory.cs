using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses
{
    public class PatientHistory
    {
        public string Type { get; set; }
        public DateTime AddedOn { get; set; }
        public string Description { get; set; }
    }
}
