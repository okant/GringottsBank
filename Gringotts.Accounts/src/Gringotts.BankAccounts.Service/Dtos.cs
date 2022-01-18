using System;

namespace Gringotts.BankAccounts.Service.Dtos
{
    public record AccountDto(Guid Id, Guid UserId, Guid CustomerId, string Name, string Surname, string AccountNo, string AccountDescription, decimal AccountBalance, string Currency, DateTimeOffset AccountOpenDate);

    public record CreateAccountDto(Guid UserId, Guid CustomerId, string AccountNo, string AccountDescription, decimal AccountBalance, string Currency);

    public record CustomerDto(Guid Id, string Name, string Surname);
}