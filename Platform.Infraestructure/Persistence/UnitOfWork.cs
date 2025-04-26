using Platform.Core.Interfaces;
using Platform.Infraestructure.Data;
using Platform.Infraestructure.Repositories;

namespace Platform.Infraestructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
  private readonly PlatformContext _context;
  public UnitOfWork(PlatformContext context)
  {
    _context = context;

    Wallets = new WalletRepository(_context);
    Movements = new MovementRepository(_context);
  }

  public IWalletRepository Wallets { get; private set; }
  public IMovementRepository Movements { get; private set; }

  public async Task<int> CompleteAsync()
  {
    return await _context.SaveChangesAsync();
  }

  public void Dispose()
  {
    _context.Dispose();
  }
}
