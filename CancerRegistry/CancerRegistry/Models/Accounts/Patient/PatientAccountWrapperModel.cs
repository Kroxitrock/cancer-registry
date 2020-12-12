using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Patient
{
    public class PatientAccountWrapperModel
    {
        public PatientLoginModel LoginModel { get; set; }
        public PatientRegisterModel RegisterModel { get; set; }
    }
}
