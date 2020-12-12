using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Patient
{
    public class PatientRegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EGN { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string RepeatPassoword { get; set; }
    }
}
