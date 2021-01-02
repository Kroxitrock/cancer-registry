using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses.HealthChecks
{
    public class HealthCheck
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public Diagnose Diagnose { get; set; }
    }
}
