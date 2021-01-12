using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Patient
{
    public class PatientLoginModel
    {
        [Required(ErrorMessage = "Полето ЕГН е задължително.")]
        public string EGN { get; set; }

        [Required(ErrorMessage = "Необходима е парола."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
