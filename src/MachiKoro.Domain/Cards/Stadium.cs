using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class Stadium : Establishment
{
    private static readonly TakeFromEveryone Effect = new(Coins.Of(2));

    public Stadium(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers => ActivationNumbers.At(DiceRoll.Of(6));

    public override Category Category => Category.Purple;

    public override Industry Industry => Industry.Tower;

    public override Coins Cost => Coins.Of(6);

    protected override IIncomeEffect IncomeEffect => Effect;
}
