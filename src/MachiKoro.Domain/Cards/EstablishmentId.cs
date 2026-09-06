using System.Runtime.InteropServices;
using StronglyTypedIds;

namespace MachiKoro.Domain.Cards;

/// <summary>Identifies an establishment card instance.</summary>
[StructLayout(LayoutKind.Auto)]
[StronglyTypedId("guid-domain")]
public readonly partial struct EstablishmentId;
