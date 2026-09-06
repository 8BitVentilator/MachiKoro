using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class ConvenienceStore : Establishment
{
    private static readonly FixedIncomeFromBank Effect = new(Coins.Of(3));

    public ConvenienceStore(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(4));

    public override Category Category => Category.Green;

    public override Industry Industry => Industry.Shop;

    public override Coins Cost => Coins.Of(2);

    protected override IIncomeEffect IncomeEffect => Effect;
}
