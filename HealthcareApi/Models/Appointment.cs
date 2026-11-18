namespace HealthcareApi.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public User Patient { get; set; } = null!;

    public int TimeSlotId { get; set; }
    public TimeSlot TimeSlot { get; set; } = null!;

    public string Symptoms { get; set; } = string.Empty;
    public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
}