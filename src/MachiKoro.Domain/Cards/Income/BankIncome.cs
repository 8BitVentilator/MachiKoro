using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Represents income paid by the bank.</summary>
/// <param name="Amount">The amount paid by the bank.</param>
public sealed record BankIncome(Coins Amount) : IncomeClaim;
