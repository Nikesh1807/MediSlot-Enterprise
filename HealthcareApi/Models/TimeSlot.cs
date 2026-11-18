namespace HealthcareApi.Models;

public class TimeSlot
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public bool IsBooked { get; set; } = false;
    public bool IsLocked { get; set; } = false; // Jab user checkout page par ho, tab temporary lock karne ke liye
    public DateTime? LockExpiresAt { get; set; } // Lock kitne time tak valid rahega
}