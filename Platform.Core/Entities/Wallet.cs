namespace Platform.Core.Entities;

public class Wallet : BaseEntity
{
  public string DocumentId { get; set; }
  public string Name { get; set; }
  public decimal Balance { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

  public ICollection<Movement>? Movements { get; set; }
}
