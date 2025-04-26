using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Platform.Api.DTOs;
using Platform.Core.Entities;
using Platform.Core.Interfaces;

namespace Platform.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletsController : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper _mapper;

  public WalletsController(IUnitOfWork unitOfWork, IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var wallets = await _unitOfWork.Wallets.GetAllAsync();
    return Ok(wallets);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var wallet = await _unitOfWork.Wallets.GetByIdAsync(id);
    if (wallet == null)
      return NotFound();
    return Ok(wallet);
  }

  [HttpPost]
  public async Task<IActionResult> Create(WalletDto dto)
  {
    var wallet = _mapper.Map<Wallet>(dto);
    wallet.CreatedAt = DateTime.UtcNow;
    await _unitOfWork.Wallets.AddAsync(wallet);
    await _unitOfWork.CompleteAsync();

    return CreatedAtAction(nameof(GetById), new { id = wallet.Id }, wallet);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, WalletDto dto)
  {
    var wallet = _mapper.Map<Wallet>(dto);

    var updated = await _unitOfWork.Wallets.UpdateAsync(id, wallet);
    if (!updated)
      return NotFound();

    await _unitOfWork.CompleteAsync();
    return NoContent();
  }


  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    await _unitOfWork.Wallets.DeleteAsync(id);
    await _unitOfWork.CompleteAsync();

    return NoContent();
  }
}
