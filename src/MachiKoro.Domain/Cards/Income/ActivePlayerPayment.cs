using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed record ActivePlayerPayment(Coins Amount) : IncomeClaim;
