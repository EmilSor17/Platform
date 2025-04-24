using Microsoft.EntityFrameworkCore;
using Platform.Core.Entities;
using Platform.Infraestructure.Data;
using Platform.Infraestructure.Repositories;
using FluentAssertions;

public class WalletRepositoryTests
{
  private PlatformContext GetInMemoryContext()
  {
    var options = new DbContextOptionsBuilder<PlatformContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    return new PlatformContext(options);
  }

  /// <summary>
  /// Comentar db context en program.cs para correr test
  /// </summary>

  [Fact]
  public async Task AddAsync_ShouldCreateWallet_WhenDocumentIdIsUnique()
  {
    var context = GetInMemoryContext();
    var repo = new WalletRepository(context);

    var wallet = new Wallet
    {
      Name = "Test Wallet",
      DocumentId = "ABC123",
      Balance = 100
    };

    var result = await repo.AddAsync(wallet);

    result.Should().NotBeNull();
    result.DocumentId.Should().Be("ABC123");
  }

  [Fact]
  public async Task AddAsync_ShouldThrow_WhenDocumentIdAlreadyExists()
  {
    var context = GetInMemoryContext();
    var repo = new WalletRepository(context);

    var wallet = new Wallet
    {
      Name = "Original Wallet",
      DocumentId = "DUP123",
      Balance = 50
    };

    await repo.AddAsync(wallet);
    await context.SaveChangesAsync();


    var duplicateWallet = new Wallet
    {
      Name = "Duplicate Wallet",
      DocumentId = "DUP123",
      Balance = 200
    };

    Func<Task> act = async () => await repo.AddAsync(duplicateWallet);
    await act.Should().ThrowAsync<InvalidOperationException>()
        .WithMessage("*already exists*");
  }

  [Fact]
  public async Task UpdateAsync_ShouldUpdateWallet_WhenWalletExists()
  {
    var context = GetInMemoryContext();
    var repo = new WalletRepository(context);

    var wallet = await repo.AddAsync(new Wallet
    {
      Name = "Old Name",
      DocumentId = "X123",
      Balance = 100
    });

    var updated = new Wallet
    {
      Name = "New Name",
      DocumentId = "X123",
      Balance = 200
    };

    var success = await repo.UpdateAsync(wallet.Id, updated);

    success.Should().BeTrue();

    var updatedWallet = await repo.GetByIdAsync(wallet.Id);
    updatedWallet?.Name.Should().Be("New Name");
    updatedWallet?.Balance.Should().Be(200);
  }

  [Fact]
  public async Task UpdateAsync_ShouldReturnFalse_WhenWalletNotFound()
  {
    var context = GetInMemoryContext();
    var repo = new WalletRepository(context);

    var fakeWallet = new Wallet
    {
      Name = "Fake",
      DocumentId = "Z999",
      Balance = 0
    };

    var result = await repo.UpdateAsync(999, fakeWallet);
    result.Should().BeFalse();
  }
}
