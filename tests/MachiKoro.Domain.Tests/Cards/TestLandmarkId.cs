using MachiKoro.Domain.Cards;

namespace MachiKoro.Domain.Tests.Cards;

internal static class TestLandmarkId
{
    public static LandmarkId Create() =>
        new(Guid.Parse("8c7390bb-a732-4765-adc0-215902f28644"));
}
