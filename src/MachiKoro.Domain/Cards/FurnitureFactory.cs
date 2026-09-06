using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class FurnitureFactory : Establishment
{
    private static readonly IncomePerIndustry Effect = new(Industry.Gear, Coins.Of(3));

    public FurnitureFactory(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(8));

    public override Category Category => Category.Green;

    public override Industry Industry => Industry.Factory;

    public override Coins Cost => Coins.Of(3);

    protected override IIncomeEffect IncomeEffect => Effect;
}
