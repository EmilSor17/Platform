using Platform.Core.Enums;

namespace Platform.Core.Entities
{
  public class Movement : BaseEntity
  {
    public int WalletId { get; set; }
    public Wallet Wallet { get; set; }

    public decimal Amount { get; set; }
    public MovementType Type { get; set; }
    public DateTime CreatedAt { get; set; }
  }

}
