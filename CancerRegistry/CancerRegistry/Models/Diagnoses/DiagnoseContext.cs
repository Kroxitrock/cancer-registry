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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<long>("Diagnoses_seq", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Diagnose>()
                .Property(d => d.Id)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.Diagnoses_seq");


            modelBuilder.HasSequence<long>("HealthChecks_seq", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<HealthCheck>()
                .Property(d => d.Id)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.HealthChecks_seq");


            modelBuilder.HasSequence<long>("Treatments_seq", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Treatment>()
                .Property(d => d.Id)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.Treatments_seq");
        }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<HealthCheck> HealthChecks { get; set; } 

        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }
    }
}
