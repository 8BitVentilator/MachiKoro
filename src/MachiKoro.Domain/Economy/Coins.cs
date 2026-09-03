using System.Globalization;

namespace MachiKoro.Domain.Economy;

/// <summary>
/// Münzvorrat eines Spielers oder Kaufpreis eines Gebäudes. Kann nie negativ werden.
/// </summary>
public readonly record struct Coins
{
    public static readonly Coins Zero = new(0);

    private readonly int _amount;

    private Coins(int amount)
    {
        _amount = amount;
    }

    public static Coins Of(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        return new Coins(amount);
    }

    public Coins Receive(Coins income) => new(_amount + income._amount);

    /// <summary>
    /// Zahlt so viel wie möglich. Wer nicht genug hat, zahlt nach den Spielregeln alles, was er besitzt.
    /// </summary>
    public Coins Pay(Coins cost) => new(Math.Max(0, _amount - cost._amount));

    public bool CanAfford(Coins cost) => _amount >= cost._amount;

    public override string ToString() => _amount.ToString(CultureInfo.InvariantCulture);
}
