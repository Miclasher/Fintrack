using Fintrack.Services.Abstractions;
using Fintrack.Shared;
using Fintrack.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionTypeController : ControllerBase
{
    private readonly ITransactionTypeService _transactionTypeService;

    public TransactionTypeController(ITransactionTypeService transactionTypeeService)
    {
        _transactionTypeService = transactionTypeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionTypeDTO>>> GetAll()
    {
        var userId = User.GetUserIdFromJwt();

        var trTypes = await _transactionTypeService.GetAllAsync(userId);

        return Ok(trTypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionTypeDTO>> GetById(Guid id)
    {
        var userId = User.GetUserIdFromJwt();

        var trType = await _transactionTypeService.GetByIdAsync(userId, id);

        return Ok(trType);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var userId = User.GetUserIdFromJwt();

        await _transactionTypeService.DeleteAsync(userId, id);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Update(TransactionTypeDTO transactionType)
    {
        var userId = User.GetUserIdFromJwt();

        await _transactionTypeService.UpdateAsync(userId, transactionType);

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(TransactionTypeForCreateDTO transactionType)
    {
        var userId = User.GetUserIdFromJwt();

        var newTransTypeid = await _transactionTypeService.CreateAsync(userId, transactionType);

        return Ok(newTransTypeid);
    }
}
