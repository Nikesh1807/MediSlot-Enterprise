using Microsoft.EntityFrameworkCore;
using HealthcareApi.Models;

namespace HealthcareApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Doctors
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 1, Name = "Dr. Sharma", Specialization = "General Physician" },
            new Doctor { Id = 2, Name = "Dr. Priya Patel", Specialization = "Cardiologist" },
            new Doctor { Id = 3, Name = "Dr. Rajesh Mehta", Specialization = "Dermatologist" },
            new Doctor { Id = 4, Name = "Dr. Ananya Roy", Specialization = "Pediatrician" },
            new Doctor { Id = 5, Name = "Dr. Vikram Joshi", Specialization = "Orthopedic Surgeon" },
            new Doctor { Id = 6, Name = "Dr. Neha Gupta", Specialization = "Neurologist" }
        );

        // Seed Time Slots using DateTime
        // Seed Time Slots using UTC DateTime
        modelBuilder.Entity<TimeSlot>().HasData(
            // Dr. Sharma (Id: 1)
            new TimeSlot { Id = 1, DoctorId = 1, StartTime = new DateTime(2026, 11, 25, 9, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 9, 30, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },
            new TimeSlot { Id = 2, DoctorId = 1, StartTime = new DateTime(2026, 11, 25, 9, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 10, 0, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },
            new TimeSlot { Id = 3, DoctorId = 1, StartTime = new DateTime(2026, 11, 25, 10, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 10, 30, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },

            // Dr. Priya Patel (Id: 2)
            new TimeSlot { Id = 4, DoctorId = 2, StartTime = new DateTime(2026, 11, 25, 11, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 11, 30, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },
            new TimeSlot { Id = 5, DoctorId = 2, StartTime = new DateTime(2026, 11, 25, 11, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 12, 0, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },

            // Dr. Rajesh Mehta (Id: 3)
            new TimeSlot { Id = 6, DoctorId = 3, StartTime = new DateTime(2026, 11, 25, 14, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 14, 30, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },
            new TimeSlot { Id = 7, DoctorId = 3, StartTime = new DateTime(2026, 11, 25, 14, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 15, 0, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },

            // Dr. Ananya Roy (Id: 4)
            new TimeSlot { Id = 8, DoctorId = 4, StartTime = new DateTime(2026, 11, 25, 16, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 16, 30, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },
            new TimeSlot { Id = 9, DoctorId = 4, StartTime = new DateTime(2026, 11, 25, 16, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 17, 0, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },

            // Dr. Vikram Joshi (Id: 5)
            new TimeSlot { Id = 10, DoctorId = 5, StartTime = new DateTime(2026, 11, 25, 10, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 10, 30, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },
            new TimeSlot { Id = 11, DoctorId = 5, StartTime = new DateTime(2026, 11, 25, 10, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 11, 0, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },

            // Dr. Neha Gupta (Id: 6)
            new TimeSlot { Id = 12, DoctorId = 6, StartTime = new DateTime(2026, 11, 25, 17, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 17, 30, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false },
            new TimeSlot { Id = 13, DoctorId = 6, StartTime = new DateTime(2026, 11, 25, 17, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2026, 11, 25, 18, 0, 0, DateTimeKind.Utc), IsBooked = false, IsLocked = false }
        );
    }
}