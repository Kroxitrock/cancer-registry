using CancerRegistry.Models.Diagnoses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Accounts.Patient
{
    public class Patient
    {
        [Key]
        [Required]
        public string UserId { get; set; }

        [Required]
        public int PhoneNumber { get; set; }


        [Required]
        [ForeignKey("Diagnose")]
        public long ActiveDiagnoseId { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }

    }
}
