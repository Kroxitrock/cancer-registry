using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses
{
    public class DiagnoseModel
    {
        public long Id { get; set; }

        public bool DiagnoseExists { get; set; }

        public string PatientId { get; set; }

        public string PatientName { get; set; }

        public short Stage { get; set; }

        public PrimaryTumorState PrimaryTumor { get; set; }

        public DistantMetastasisState DistantMetastasis { get; set; }

        public RegionalLymphNodesState RegionalLymphNodes { get; set; }
    }
}
