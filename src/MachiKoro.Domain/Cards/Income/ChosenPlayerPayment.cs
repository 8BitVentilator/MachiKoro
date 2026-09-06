using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Represents a payment owed by a player selected by the claimant.</summary>
/// <param name="Amount">The amount owed by the selected player.</param>
public sealed record ChosenPlayerPayment(Coins Amount) : IncomeClaim;
