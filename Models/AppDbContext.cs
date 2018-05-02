using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FYPHomecare.Models
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Quote> Quote { get; set; }
        public virtual DbSet<ServiceProvider> ServiceProvider { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequest { get; set; }
        public virtual DbSet<ServiceType> ServiceType { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.AnnualIncome)
                    .HasColumnName("annual_income")
                    .HasColumnType("decimal");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .HasColumnType("date");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasColumnType("char(1)");

                entity.Property(e => e.MedicalHistory)
                    .IsRequired()
                    .HasColumnName("medical_history")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Nric)
                    .IsRequired()
                    .HasColumnName("nric")
                    .HasColumnType("varchar(9)");

                entity.Property(e => e.Phone).HasColumnName("phone");

                entity.Property(e => e.Postalcode).HasColumnName("postalcode");

                entity.Property(e => e.RelationshipToUser)
                    .HasColumnName("relationship_to_user")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.UsersId).HasColumnName("users_id");
            });

            modelBuilder.Entity<Quote>(entity =>
            {
                entity.Property(e => e.QuoteId).HasColumnName("quote_id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Price).HasColumnType("decimal");

                entity.Property(e => e.ServiceProviderId).HasColumnName("Service_Provider_id");

                entity.Property(e => e.UsersId).HasColumnName("users_id");

                entity.HasOne(d => d.ServiceProvider)
                    .WithMany(p => p.Quote)
                    .HasForeignKey(d => d.ServiceProviderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Quote_Service_Provider");

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Quote)
                    .HasForeignKey(d => d.UsersId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Quote_users");
            });

            modelBuilder.Entity<ServiceProvider>(entity =>
            {
                entity.ToTable("Service_Provider");

                entity.Property(e => e.ServiceProviderId).HasColumnName("service_provider_id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasColumnName("company")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("date_of_birth")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasColumnType("char(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Nric)
                    .IsRequired()
                    .HasColumnName("nric")
                    .HasColumnType("varchar(9)");

                entity.Property(e => e.Phone).HasColumnName("phone");
            });

            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.ToTable("Service_Request");

                entity.Property(e => e.ServiceRequestId).HasColumnName("service_request_id");

                entity.Property(e => e.AppointmentDatetime)
                    .HasColumnName("appointment_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.RequestDate)
                    .HasColumnName("request_date")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("varchar(45)");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.ServiceRequest)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Service_Request_Patient");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("Service_Type");

                entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");

                entity.Property(e => e.Rate)
                    .HasColumnName("rate")
                    .HasColumnType("decimal");

                entity.Property(e => e.ServiceProviderId).HasColumnName("service_provider_id");

                entity.HasOne(d => d.ServiceProvider)
                    .WithMany(p => p.ServiceType)
                    .HasForeignKey(d => d.ServiceProviderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_Service_Provider_Service_Type");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.UsersId).HasColumnName("users_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Nric)
                    .IsRequired()
                    .HasColumnName("nric")
                    .HasColumnType("varchar(9)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasColumnName("payment_method")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Phone).HasColumnName("phone");
            });
        }
    }
}