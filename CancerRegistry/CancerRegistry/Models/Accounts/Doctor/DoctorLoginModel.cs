using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Doctor
{
    public class DoctorLoginModel
    {
        public string UID { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
