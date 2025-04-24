using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Platform.Core.Entities;
using Platform.Infraestructure.Data;
using Platform.Infraestructure.Repositories;

public class MovementRepositoryTests
{
  private PlatformContext GetInMemoryContext()
  {
    var options = new DbContextOptionsBuilder<PlatformContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    return new PlatformContext(options);
  }

  private Wallet CreateWallet(string name, decimal balance, string documentId)
  {
    return new Wallet
    {
      Name = name,
      Balance = balance,
      DocumentId = documentId
    };
  }


  /// <summary>
  /// Comentar db context en program.cs para correr test
  /// </summary>
  
  [Fact]
  public async Task TransferAsync_ShouldTransfer_WhenValid()
  {
    var context = GetInMemoryContext();
    var origin = CreateWallet("Origin", 500, "DOC1");
    var dest = CreateWallet("Destination", 100, "DOC2");

    context.Wallets.AddRange(origin, dest);
    await context.SaveChangesAsync();

    var repo = new MovementRepository(context);

    var result = await repo.TransferAsync(origin.Id, dest.Id, 200);
    await context.SaveChangesAsync();

    result.Success.Should().BeTrue();

    var updatedOrigin = await context.Wallets.FindAsync(origin.Id);
    var updatedDest = await context.Wallets.FindAsync(dest.Id);

    updatedOrigin?.Balance.Should().Be(300);
    updatedDest?.Balance.Should().Be(300);

    var movements = await context.Movements.ToListAsync();
    movements.Should().HaveCount(2);
  }

  [Fact]
  public async Task TransferAsync_ShouldFail_WhenInsufficientFunds()
  {
    var context = GetInMemoryContext();
    var origin = CreateWallet("Origin", 50, "DOC3");
    var dest = CreateWallet("Destination", 100, "DOC4");

    context.Wallets.AddRange(origin, dest);
    await context.SaveChangesAsync();

    var repo = new MovementRepository(context);

    var result = await repo.TransferAsync(origin.Id, dest.Id, 100);

    result.Success.Should().BeFalse();
    result.Message.Should().Be("Insufficient funds.");
  }

  [Fact]
  public async Task TransferAsync_ShouldFail_WhenWalletNotFound()
  {
    var context = GetInMemoryContext();
    var origin = CreateWallet("Origin", 100, "DOC5");

    context.Wallets.Add(origin);
    await context.SaveChangesAsync();

    var repo = new MovementRepository(context);

    var result = await repo.TransferAsync(origin.Id, 999, 50);

    result.Success.Should().BeFalse();
    result.Message.Should().Be("One or both wallets not found.");
  }

  [Fact]
  public async Task TransferAsync_ShouldFail_WhenAmountInvalid()
  {
    var context = GetInMemoryContext();
    var origin = CreateWallet("Origin", 500, "DOC6");
    var dest = CreateWallet("Destination", 500, "DOC7");

    context.Wallets.AddRange(origin, dest);
    await context.SaveChangesAsync();

    var repo = new MovementRepository(context);

    var result = await repo.TransferAsync(origin.Id, dest.Id, 0);
    result.Success.Should().BeFalse();
    result.Message.Should().Be("Amount must be greater than zero.");
  }
}
