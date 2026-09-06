using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class BusinessCenter : Establishment
{
    private static readonly SwapEstablishment Effect = new();

    public BusinessCenter(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(6));

    public override Category Category => Category.Purple;

    public override Industry Industry => Industry.Tower;

    public override Coins Cost => Coins.Of(8);

    protected override IIncomeEffect IncomeEffect => Effect;
}
