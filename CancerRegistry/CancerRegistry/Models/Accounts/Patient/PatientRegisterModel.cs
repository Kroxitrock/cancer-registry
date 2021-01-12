using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Patient
{
    public class PatientRegisterModel
    {
        [Required(ErrorMessage = "Полето \"Име\" е задължнително.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Полето \"Фамилия\" е задължително.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Полето \"ЕГН\" е задължително.")]
        public string EGN { get; set; }

        [Required(ErrorMessage = "Полето \"Телефонен номер\" е задължително."), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна."), DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string RepeatPassoword { get; set; }
    }
}
