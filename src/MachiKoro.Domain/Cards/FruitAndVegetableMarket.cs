using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class FruitAndVegetableMarket : Establishment
{
    private static readonly IncomePerIndustry Effect = new(Industry.Wheat, Coins.Of(2));

    public FruitAndVegetableMarket(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers =>
        ActivationNumbers.Between(DiceRoll.Of(11), DiceRoll.Of(12));

    public override Category Category => Category.Green;

    public override Industry Industry => Industry.Fruit;

    public override Coins Cost => Coins.Of(2);

    protected override IIncomeEffect IncomeEffect => Effect;
}
