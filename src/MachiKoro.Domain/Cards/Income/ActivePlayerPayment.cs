using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Represents a payment owed by the active player.</summary>
/// <param name="Amount">The amount owed by the active player.</param>
public sealed record ActivePlayerPayment(Coins Amount) : IncomeClaim;
