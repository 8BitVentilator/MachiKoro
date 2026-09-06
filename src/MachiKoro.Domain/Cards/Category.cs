namespace MachiKoro.Domain.Cards;

/// <summary>Specifies when and for whom an establishment produces income.</summary>
public enum Category
{
    /// <summary>Produces income for every player.</summary>
    Blue,

    /// <summary>Produces income for the active player.</summary>
    Green,

    /// <summary>Takes income from the active player for an opponent.</summary>
    Red,

    /// <summary>Applies a major establishment effect for the active player.</summary>
    Purple,
}
