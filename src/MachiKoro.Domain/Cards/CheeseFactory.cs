using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class CheeseFactory : Establishment
{
    private static readonly IncomePerIndustry Effect = new(Industry.Cow, Coins.Of(3));

    public CheeseFactory(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceRoll.Of(7));

    public override Category Category => Category.Green;

    public override Industry Industry => Industry.Factory;

    public override Coins Cost => Coins.Of(5);

    protected override IIncomeEffect IncomeEffect => Effect;
}
