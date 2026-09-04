using MachiKoro.Domain.Cards;

namespace MachiKoro.Domain.Tests.Cards;

internal static class TestEstablishmentId
{
    public static EstablishmentId Create() =>
        new(Guid.Parse("860675c5-e78a-4cf0-a2cb-25cce10b0f76"));
}
