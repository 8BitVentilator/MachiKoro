using System.Runtime.InteropServices;
using StronglyTypedIds;

namespace MachiKoro.Domain.Cards;

/// <summary>Identifies a landmark card instance.</summary>
[StructLayout(LayoutKind.Auto)]
[StronglyTypedId("guid-domain")]
public readonly partial struct LandmarkId;
