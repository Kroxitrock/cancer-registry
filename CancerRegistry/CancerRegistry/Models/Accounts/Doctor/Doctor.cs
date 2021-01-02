using CancerRegistry.Models.Diagnoses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Doctor
{
    public class Doctor
    {
        [Key]
        [Required]
        public string  UserId { get; set; }

        [Required]
        public string EGN { get; set; }

        [Required]
        public string DiplomaNum { get; set; }

        [Required]
        public string EIK { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }
    }
}
