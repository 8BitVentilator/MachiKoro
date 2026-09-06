using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class Cafe : Establishment
{
    private static readonly TakeFromActivePlayer Effect = new(Coins.Of(1));

    public Cafe(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceTotal.Of(3));

    public override Category Category => Category.Red;

    public override Industry Industry => Industry.Cup;

    public override Coins Cost => Coins.Of(2);

    protected override IIncomeEffect IncomeEffect => Effect;
}
