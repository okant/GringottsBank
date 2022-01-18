using Gringotts.BankAccounts.Service.Dtos;
using Gringotts.BankAccounts.Service.Entities;

namespace Gringotts.BankAccounts.Service
{
    public static class Extensions
    {
        public static AccountDto AsDto(this Account account, string name, string surname) => new AccountDto(account.Id, account.UserId, account.CustomerId, name, surname, account.AccountNo, account.AccountDescription, account.AccountBalance, account.Currency, account.AccountOpenDate);
    }
}