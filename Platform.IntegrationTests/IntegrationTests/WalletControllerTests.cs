using FluentAssertions;
using System.Net.Http.Json;

public class WalletControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public WalletControllerTests(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }


  /// <summary>
  /// Comentar db context en program.cs para correr test
  /// </summary>
  [Fact]
  public async Task CreateWallet_ShouldReturnCreatedWallet()
  {
    var wallet = new
    {
      Name = "Test Wallet",
      DocumentId = "ABC123",
      Balance = 100
    };

    var response = await _client.PostAsJsonAsync("/api/Wallets", wallet);

    response.EnsureSuccessStatusCode();
    var json = await response.Content.ReadAsStringAsync();
    json.Should().Contain("Test Wallet");
  }
}
