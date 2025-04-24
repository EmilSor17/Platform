using Platform.Core.Entities;

namespace Platform.Core.Interfaces
{
  public interface IMovementRepository : IRepository<Movement>
  {
    Task<IEnumerable<Movement>> GetMovementsByWalletIdAsync(int walletId);
    Task<(bool Success, string Message)> TransferAsync(
        int originWalletId, int destinationWalletId, decimal amount);
  }
}
