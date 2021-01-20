using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Doctor
{
    public class DoctorEditProfileModel
    {
        public String Id { get; set; }

        [Required(ErrorMessage = "Полето \"Име\" е задължнително.")]
        public String FirstName { get; set; }
        
        [Required(ErrorMessage = "Полето \"Фамилия\" е задължително.")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Полето \"ЕГН\" е задължително."), StringLength(10, MinimumLength = 10, ErrorMessage = "Въведеното ЕГН е невалидно.")]
        public String EGN { get; set; }

        [Required(ErrorMessage = "Полето \"Телефонен номер\" е задължително."), DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public String Gender { get; set; }

        public String[] Genders = new String[] { "Мъж", "Жена" };
    }
}
