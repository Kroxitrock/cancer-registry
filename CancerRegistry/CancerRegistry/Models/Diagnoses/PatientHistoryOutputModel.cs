using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses
{
    public class PatientHistoryOutputModel
    {
        public string PatientName { get; set; }

        public IEnumerable<PatientHistory> History {get; set; }
    }
}
