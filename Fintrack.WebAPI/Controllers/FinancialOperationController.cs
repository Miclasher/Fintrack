using Fintrack.Services.Abstractions;
using Fintrack.Shared;
using Fintrack.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FinancialOperationController : ControllerBase
{
    private readonly IFinancialOperationService _financialOperationService;

    public FinancialOperationController(IFinancialOperationService financialOperationService)
    {
        _financialOperationService = financialOperationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FinancialOperationDTO>>> GetAll()
    {
        var userId = User.GetUserIdFromJwt();

        var finOps = await _financialOperationService.GetAllAsync(userId);

        return Ok(finOps);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FinancialOperationDTO>> GetById(Guid id)
    {
        var userId = User.GetUserIdFromJwt();

        var finOp = await _financialOperationService.GetByIdAsync(userId, id);

        return Ok(finOp);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] FinancialOperationForCreateDTO financialOperation)
    {
        var userId = User.GetUserIdFromJwt();

        var newFinOpId = await _financialOperationService.CreateAsync(userId, financialOperation);

        return Ok(newFinOpId);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] FinancialOperationDTO financialOperation)
    {
        var userId = User.GetUserIdFromJwt();

        await _financialOperationService.UpdateAsync(userId, financialOperation);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserIdFromJwt();

        await _financialOperationService.DeleteAsync(userId, id);

        return Ok();
    }
}
