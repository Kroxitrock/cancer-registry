using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Doctor
{
    public class DoctorLoginModel
    {
        [Required(ErrorMessage = "Полето \"УИН\" е задължително.")]
        public string UID { get; set; }

        [Required(ErrorMessage = "Полето \"УИН\" е задължително."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
