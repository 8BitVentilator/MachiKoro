using MachiKoro.Domain.Cards.Income;
using MachiKoro.Domain.Dice;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public abstract class Establishment
{
    protected Establishment(EstablishmentId id)
    {
        Id = id;
    }

    public EstablishmentId Id { get; }

    public abstract ActivationNumbers ActivationNumbers { get; }

    public abstract Category Category { get; }

    public abstract Industry Industry { get; }

    public abstract Coins Cost { get; }

    protected abstract IIncomeEffect IncomeEffect { get; }

    public bool IsActivatedBy(DiceRoll roll) => ActivationNumbers.Includes(roll);

    public bool HasIndustry(Industry industry) => Industry == industry;

    public IncomeClaim CreateIncomeClaim(Establishments establishments) =>
        IncomeEffect.CreateClaim(establishments);
}
