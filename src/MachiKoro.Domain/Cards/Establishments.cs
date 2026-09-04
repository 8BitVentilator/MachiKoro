using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards;

public sealed class Establishments
{
    public static readonly Establishments Empty = new([]);

    private readonly IReadOnlyCollection<Establishment> _items;

    private Establishments(IReadOnlyCollection<Establishment> items)
    {
        _items = items;
    }

    public static Establishments Of(params Establishment[] establishments) =>
        new(Array.AsReadOnly(establishments.ToArray()));

    public Coins IncomeFor(Industry industry, Coins incomePerEstablishment) =>
        _items
            .Where(establishment => establishment.HasIndustry(industry))
            .Aggregate(Coins.Zero, (income, _) => income.Receive(incomePerEstablishment));
}
