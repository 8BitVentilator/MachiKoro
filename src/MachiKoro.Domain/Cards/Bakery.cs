using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class Bakery : Establishment
{
    public Bakery(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers =>
        ActivationNumbers.Between(DiceTotal.Of(2), DiceTotal.Of(3));

    public override Category Category => Category.Green;

    public override Industry Industry => Industry.Shop;

    public override Coins Cost => Coins.Of(1);

    protected override IIncomeEffect IncomeEffect => FixedIncomeFromBank.One;
}
