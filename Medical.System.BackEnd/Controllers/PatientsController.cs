using Medical.System.Core.Models.DTOs;
using Medical.System.Core.Models.Entities;
using Medical.System.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medical.System.BackEnd.Controllers;

[Route("[controller]")]
[ApiController]
public class PatientsController: ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    // GET: api/Patients
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _patientService.GetAllPatientsAsync());
    }

    // GET: api/Patients/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var patient = await _patientService.GetPatientByIdAsync(id);
        if (patient == null)
        {
            return NotFound();
        }
        return Ok(patient);
    }

    // POST: api/Patients
    [HttpPost]
    public async Task<IActionResult> Create(PatientDto patientDto)
    {
        var patient = await _patientService.CreatePatientAsync(patientDto);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    // PUT: api/Patients/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, PatientDto patient)
    {
        await _patientService.UpdatePatientAsync(id,patient);
        return NoContent();
    }

    // PATH: api/Patients/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] Dictionary<string, object> updates)
    {
            await _patientService.UpdatePatientByPathAsync(id, updates);
            return NoContent();
    }

    // DELETE: api/Patients/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _patientService.DeletePatientAsync(id);
        return NoContent();
    }
}
