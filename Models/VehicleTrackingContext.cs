using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VehicleAPI.Models
{
    public partial class VehicleTrackingContext : DbContext
    {
        public VehicleTrackingContext()
        {
        }

        public VehicleTrackingContext(DbContextOptions<VehicleTrackingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-RLJ65LJ;Initial Catalog=VehicleTracking;User Id=sa;Password=msbsql2020update");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

                entity.Property(e => e.Latitude).HasColumnType("decimal(10, 8)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(11, 8)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.UserEmail).HasMaxLength(250);

                entity.Property(e => e.UserName).HasMaxLength(250);

                entity.Property(e => e.UserRole).HasMaxLength(100);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicle");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Accesstime).HasColumnType("datetime");

                entity.Property(e => e.RegisterNo).HasMaxLength(250);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VehicleName).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
