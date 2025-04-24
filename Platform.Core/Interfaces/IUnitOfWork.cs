namespace Platform.Core.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IWalletRepository Wallets { get; }
    IMovementRepository Movements { get; }
    Task<int> CompleteAsync();
  }
}
