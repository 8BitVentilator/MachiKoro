using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class Mine : Establishment
{
    private static readonly FixedIncomeFromBank Effect = new(Coins.Of(5));

    public Mine(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceRoll.Of(9));

    public override Category Category => Category.Blue;

    public override Industry Industry => Industry.Gear;

    public override Coins Cost => Coins.Of(6);

    protected override IIncomeEffect IncomeEffect => Effect;
}
