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

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String EGN { get; set; }

        public String PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public String Gender { get; set; }

        public String[] Genders = new String[] { "Мъж", "Жена" };
    }
}
