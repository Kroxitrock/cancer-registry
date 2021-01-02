using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Models.Diagnoses.HealthChecks;
using CancerRegistry.Models.Diagnoses.Treatments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Models.Diagnoses
{
    public class DiagnoseContext : DbContext
    {
        public DiagnoseContext(DbContextOptions<DiagnoseContext> options) : base(options)
        {

        }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<HealthCheck> HealthChecks { get; set; } 

        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }
    }
}
