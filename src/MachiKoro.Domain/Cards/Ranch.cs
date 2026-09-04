using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class Ranch : Establishment
{
    public Ranch(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceRoll.Of(2));

    public override Category Category => Category.Blue;

    public override Industry Industry => Industry.Cow;

    public override Coins Cost => Coins.Of(1);

    protected override IIncomeEffect IncomeEffect => FixedIncomeFromBank.One;
}
