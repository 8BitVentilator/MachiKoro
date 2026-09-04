using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class FamilyRestaurant : Establishment
{
    private static readonly TakeFromActivePlayer Effect = new(Coins.Of(2));

    public FamilyRestaurant(EstablishmentId id)
        : base(id)
    {
    }

    public override ActivationNumbers ActivationNumbers =>
        ActivationNumbers.Between(DiceRoll.Of(9), DiceRoll.Of(10));

    public override Category Category => Category.Red;

    public override Industry Industry => Industry.Cup;

    public override Coins Cost => Coins.Of(3);

    protected override IIncomeEffect IncomeEffect => Effect;
}
