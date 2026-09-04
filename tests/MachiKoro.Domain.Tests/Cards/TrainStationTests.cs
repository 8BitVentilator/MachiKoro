using MachiKoro.Domain.Cards;
using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Tests.Cards;

public sealed class TrainStationTests
{
    [Fact]
    public void Id_DifferentCopies_IsIndividual()
    {
        TrainStation first = new(new LandmarkId(Guid.Parse("4e3f8132-44ce-4925-89fc-d003f4f07d36")));
        TrainStation second = new(new LandmarkId(Guid.Parse("dd0e47cf-c6f0-4632-8c81-5a914fa890f9")));

        first.Id.ShouldNotBe(second.Id);
    }

    [Fact]
    public void Cost_ReturnsFourCoins()
    {
        TrainStation station = new(TestLandmarkId.Create());

        station.Cost.ShouldBe(Coins.Of(4));
    }
}
