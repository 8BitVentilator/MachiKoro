using MachiKoro.Domain.Economy;

namespace MachiKoro.Domain.Cards.Income;

public sealed record BankIncome(Coins Amount) : IncomeClaim;
