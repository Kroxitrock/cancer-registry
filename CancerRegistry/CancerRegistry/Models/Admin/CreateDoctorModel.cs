using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Admin
{
    public class CreateDoctorModel
    {
        [Required(ErrorMessage = "Полето \"Име\" е задължително.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Полето \"Фамилия\" е задължително.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Полето \"ЕГН\" е задължително.")]
        public string EGN { get; set; }

        [Required(ErrorMessage = "Полето \"УИН\" е задължително.")]
        public string UID { get; set; }

        [Required(ErrorMessage = "Полето \"БУЛСТАТ\" е задължително.")]
        public string Bulstat { get; set; }
        
        public String Gender { get; set; }

        public String[] Genders = new String[] { "Мъж", "Жена" };
    }
}
