using Platform.Core.Entities;

namespace Platform.Core.Interfaces;

public interface IWalletRepository : IRepository<Wallet>
{
  Task<Wallet?> GetByDocumentIdAsync(string documentId);
  Task<bool> UpdateAsync(int id, Wallet wallet);
}
