using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

internal sealed record EstablishmentExpectation(
    ActivationNumbers ActivationNumbers,
    Category Category,
    Industry Industry,
    Coins Cost)
{
    public static EstablishmentExpectation From(Establishment establishment) =>
        new(
            establishment.ActivationNumbers,
            establishment.Category,
            establishment.Industry,
            establishment.Cost);
}
