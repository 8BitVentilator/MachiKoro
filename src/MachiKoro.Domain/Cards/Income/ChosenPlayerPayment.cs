using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed record ChosenPlayerPayment(Coins Amount) : IncomeClaim;
