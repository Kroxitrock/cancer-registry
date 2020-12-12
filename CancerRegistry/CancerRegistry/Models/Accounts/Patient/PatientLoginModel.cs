using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Patient
{
    public class PatientLoginModel
    {
        [Required]
        public string EGN { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
