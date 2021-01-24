using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses.Treatments
{
    public enum DiagnosedSurgery
    {
        S1,
        S2,
        S3,
        S4
    }

    public enum DiagnosedRadiation
    {
        R0,
        R1,
        R2,
        R3,
        R4
    }

    public enum DiagnosedChemeotherapy
    {
        C1,
        C2,
        C3,
        C4
    }

    public enum DiagnosedEndocrineTreatment
    {
        E0,
        E1,
        E2,
        E3,
        E4
    }

    public class Treatment
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public DateTime Beginning { get; set; }

        public DateTime End { get; set; }

        [Required]
        public DiagnosedSurgery Surgery { get; set; }

        [Required]
        public DiagnosedRadiation Radiation { get; set; }

        [Required]
        public DiagnosedChemeotherapy Chemeotherapy { get; set; }

        [Required]
        public DiagnosedEndocrineTreatment EndocrineTreatment { get; set; }

        [Required]
        [ForeignKey("Diagnose")]
        public long DiagnoseId { get; set; }

        public Diagnose Diagnose { get; set; }
    }
}
