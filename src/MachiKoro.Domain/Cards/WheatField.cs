using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class WheatField : Establishment
{
    public WheatField(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(1));

    public override Category Category => Category.Blue;

    public override Industry Industry => Industry.Wheat;

    public override Coins Cost => Coins.Of(1);

    protected override IIncomeEffect IncomeEffect => FixedIncomeFromBank.One;
}
