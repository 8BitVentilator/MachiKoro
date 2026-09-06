using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class AppleOrchard : Establishment
{
    private static readonly FixedIncomeFromBank Effect = new(Coins.Of(3));

    public AppleOrchard(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(10));

    public override Category Category => Category.Blue;

    public override Industry Industry => Industry.Wheat;

    public override Coins Cost => Coins.Of(3);

    protected override IIncomeEffect IncomeEffect => Effect;
}
