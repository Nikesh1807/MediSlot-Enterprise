using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthcareApi.Data;
using HealthcareApi.Models;

namespace HealthcareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DoctorsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/doctors
    [HttpGet]
    public async Task<IActionResult> GetDoctors()
    {
        var doctors = await _context.Doctors
            .Include(d => d.TimeSlots)
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.Specialization,
                TimeSlots = d.TimeSlots.Select(ts => new
                {
                    ts.Id,
                    ts.StartTime,
                    ts.EndTime,
                    ts.IsBooked,
                    ts.IsLocked,
                    ts.LockExpiresAt
                })
            })
            .ToListAsync();

        return Ok(doctors);
    }

    // POST: api/doctors
    [HttpPost]
    public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDoctors), new { id = doctor.Id }, doctor);
    }
}