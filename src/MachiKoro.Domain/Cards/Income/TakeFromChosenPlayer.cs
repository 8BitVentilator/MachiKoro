using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

/// <summary>Creates a claim for payment from a player chosen by the claimant.</summary>
public sealed class TakeFromChosenPlayer : IIncomeEffect
{
    private readonly Coins _amount;

    /// <summary>Initializes an effect with the amount requested from the chosen player.</summary>
    /// <param name="amount">The amount requested from the chosen player.</param>
    public TakeFromChosenPlayer(Coins amount)
    {
        _amount = amount;
    }

    /// <inheritdoc/>
    public IncomeClaim CreateClaim(Establishments establishments) => new ChosenPlayerPayment(_amount);
}
