using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareApi.Data;
using HealthcareApi.Models;

namespace HealthcareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeSlotsController : ControllerBase
{
    const int LockDurationMinutes = 5;
    private readonly AppDbContext _context;

    public TimeSlotsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("lock/{id}")]
    public async Task<IActionResult> LockSlot(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var slot = await _context.TimeSlots
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (slot == null)
                return NotFound("Time slot nahi mila.");

            if (slot.IsBooked)
                return BadRequest("Yeh slot already booked hai.");

            if (slot.IsLocked && slot.LockExpiresAt > DateTime.UtcNow)
                return BadRequest("Yeh slot abhi kisi aur ne lock kiya hua hai. Thodi der baad try karein.");

            slot.IsLocked = true;
            slot.LockExpiresAt = DateTime.UtcNow.AddMinutes(LockDurationMinutes);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { message = "Slot successfully lock ho gaya hai!", expiresAt = slot.LockExpiresAt });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("book")]
    public async Task<IActionResult> BookSlot([FromQuery] int patientId, [FromQuery] int slotId, [FromBody] BookingRequestDto? dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var slot = await _context.TimeSlots
                .Where(s => s.Id == slotId)
                .FirstOrDefaultAsync();

            if (slot == null)
                return NotFound("Time slot nahi mila.");

            if (slot.IsBooked)
                return BadRequest("Yeh slot pehle hi book ho chuka hai.");

            slot.IsBooked = true;
            slot.IsLocked = false;
            slot.LockExpiresAt = null;

            var appointment = new Appointment
            {
                PatientId = patientId,
                TimeSlotId = slotId,
                Symptoms = dto?.Symptoms ?? string.Empty,
                Status = "Confirmed",
                BookingDate = DateTime.UtcNow
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { message = "Appointment successfully confirm ho gayi hai!" });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("/api/appointments/cancel/{id}")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.TimeSlot)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null) return NotFound("Appointment not found.");

        appointment.Status = "Cancelled";
        appointment.TimeSlot.IsBooked = false;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Appointment cancelled successfully." });
    }

    [HttpGet("/api/appointments/patient/{patientId}")]
    public async Task<IActionResult> GetPatientAppointments(int patientId)
    {
        var list = await _context.Appointments
            .Include(a => a.TimeSlot)
            .ThenInclude(ts => ts.Doctor)
            .Where(a => a.PatientId == patientId)
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("/api/appointments/doctor/{doctorId}")]
    public async Task<IActionResult> GetDoctorAppointments(int doctorId)
    {
        // Map user login ID 2 (Dr. Sharma) to Doctors table ID 1 seamlessly
        int actualDoctorId = doctorId == 2 ? 1 : doctorId;

        var list = await _context.Appointments
            .Include(a => a.TimeSlot)
            .ThenInclude(ts => ts.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.TimeSlot.DoctorId == actualDoctorId && a.Status == "Confirmed")
            .ToListAsync();

        return Ok(list);
    }
}

public record BookingRequestDto(string Symptoms);