using Medical.System.Core.Models.DTOs;
using Medical.System.Core.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Medical.System.BackEnd.Controllers;

[Route("[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    public SuppliersController(ISupplierService supplierService)
    {
        SupplierService = supplierService;
    }
    public ISupplierService SupplierService { get; }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSupplierDto supplierDTO)
    {
        var supplier = await SupplierService.Create(supplierDTO);
        return Ok(supplier);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var supplier = await SupplierService.GetByIdAsync(id);
        return Ok(supplier);
    }
}
