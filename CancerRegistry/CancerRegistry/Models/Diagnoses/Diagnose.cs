using CancerRegistry.Models.Diagnoses.HealthChecks;
using CancerRegistry.Models.Diagnoses.Treatments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses
{
    public enum PrimaryTumorState
    {
        T1,
        T2,
        T3,
        T4
    }

    public enum DistantMetastasisState
    {
        M0,
        M1
    }

    public enum RegionalLymphNodesState
    {
        N0,
        N1,
        N2,
        N3
    }

    public class Diagnose
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public short Stage { get; set; }

        [Required]
        public PrimaryTumorState PrimaryTumor { get; set; }

        [Required]
        public DistantMetastasisState DistantMetastasis { get; set; }

        [Required]
        public RegionalLymphNodesState RegionalLymphNodes { get; set; }

        [Required]
        public ICollection<HealthCheck> HealthChecks { get; set; }

        public Treatment Treatment { get; set; }
    }
}
