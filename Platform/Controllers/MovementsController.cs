using Microsoft.AspNetCore.Mvc;
using Platform.Core.Entities;
using Platform.Core.Enums;
using Platform.Core.Interfaces;

namespace Platform.Web.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MovementsController : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork;

    public MovementsController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    [HttpGet("{walletId}")]
    public async Task<IActionResult> GetByWallet(int walletId)
    {
      var movements = await _unitOfWork.Movements.GetMovementsByWalletIdAsync(walletId);
      return Ok(movements);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(int originWalletId, int destinationWalletId, decimal amount)
    {
      var (success, message) = await _unitOfWork.Movements.TransferAsync(originWalletId, destinationWalletId, amount);

      if (!success)
        return BadRequest(message);

      await _unitOfWork.CompleteAsync();
      return Ok(message);
    }

  }
}
