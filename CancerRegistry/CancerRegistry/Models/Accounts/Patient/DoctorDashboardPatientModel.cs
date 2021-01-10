using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Patient
{
    public class DoctorDashboardPatientModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long PhoneNumber { get; set; }

        public long DiagnoseId { get; set; }

        public long LastHealthCheckId { get; set; }

        public long TreatmentId { get; set; }
    }
}
