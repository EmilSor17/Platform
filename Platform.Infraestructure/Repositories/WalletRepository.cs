using Microsoft.EntityFrameworkCore;
using Platform.Core.Entities;
using Platform.Core.Interfaces;
using Platform.Infraestructure.Data;

namespace Platform.Infraestructure.Repositories;

public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
{
  public WalletRepository(PlatformContext context)
  : base(context) 
    { }

  public async Task<Wallet?> GetByDocumentIdAsync(string documentId)
  {
    return await Query.Where(w => w.DocumentId == documentId).FirstOrDefaultAsync();
  }
  public override async Task<Wallet> AddAsync(Wallet wallet)
  {
    var exist = await GetByDocumentIdAsync(wallet.DocumentId);
    if (exist != null)      
      throw new InvalidOperationException($"A wallet with document ID '{wallet.DocumentId}' already exists.");
    
    return await base.AddAsync(wallet);
  }

  public async Task<bool> UpdateAsync(int id, Wallet wallet)
  {
    var existing = await GetByIdAsync(id);
    if (existing == null)
      return false;

    existing.Name = wallet.Name;
    existing.DocumentId = wallet.DocumentId;
    existing.Balance = wallet.Balance;
    existing.UpdatedAt = DateTime.UtcNow;

    Update(existing); 
    return true;
  }

}

