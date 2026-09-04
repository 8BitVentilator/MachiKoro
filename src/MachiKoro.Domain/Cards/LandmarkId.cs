using System.Runtime.InteropServices;
using StronglyTypedIds;

namespace MachiKoro.Domain.Cards;

[StructLayout(LayoutKind.Auto)]
[StronglyTypedId("guid-domain")]
public readonly partial struct LandmarkId;
