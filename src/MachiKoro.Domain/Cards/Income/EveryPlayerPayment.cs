using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Represents a payment owed by every applicable player.</summary>
/// <param name="Amount">The amount owed by each applicable player.</param>
public sealed record EveryPlayerPayment(Coins Amount) : IncomeClaim;
