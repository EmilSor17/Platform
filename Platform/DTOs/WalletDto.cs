﻿namespace Platform.Api.DTOs;

public class WalletDto
{
  public string DocumentId { get; set; } = null!;
  public string Name { get; set; } = null!;
  public decimal Balance { get; set; }
}
