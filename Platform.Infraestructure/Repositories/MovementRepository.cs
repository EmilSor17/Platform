using Microsoft.EntityFrameworkCore;
using Platform.Core.Entities;
using Platform.Core.Enums;
using Platform.Core.Interfaces;
using Platform.Infraestructure.Data;

namespace Platform.Infraestructure.Repositories
{
  public class MovementRepository : BaseRepository<Movement>, IMovementRepository
  {
    public MovementRepository(PlatformContext context) : base(context) { }

    public async Task<IEnumerable<Movement>> GetMovementsByWalletIdAsync(int walletId)
    {
      return await Query
          .Where(m => m.WalletId == walletId)
          .OrderByDescending(m => m.CreatedAt)
          .ToListAsync();
    }

    public async Task<(bool Success, string Message)> TransferAsync(
        int originWalletId, int destinationWalletId, decimal amount)
    {
      if (amount <= 0)
        return (false, "Amount must be greater than zero.");

      var originWallet = await _context.Wallets.FindAsync(originWalletId);
      var destinationWallet = await _context.Wallets.FindAsync(destinationWalletId);

      if (originWallet == null || destinationWallet == null)
        return (false, "One or both wallets not found.");

      if (originWallet.Balance < amount)
        return (false, "Insufficient funds.");

      originWallet.Balance -= amount;
      destinationWallet.Balance += amount;
      originWallet.UpdatedAt = DateTime.UtcNow;
      destinationWallet.UpdatedAt = DateTime.UtcNow;

      var now = DateTime.UtcNow;
      await _context.Movements.AddRangeAsync(new List<Movement>
        {
            new Movement
            {
                WalletId = originWallet.Id,
                Amount = amount,
                Type = MovementType.Debit,
                CreatedAt = now
            },
            new Movement
            {
                WalletId = destinationWallet.Id,
                Amount = amount,
                Type = MovementType.Credit,
                CreatedAt = now
            }
        });

      return (true, "Transfer successful.");
    }
  }
}
