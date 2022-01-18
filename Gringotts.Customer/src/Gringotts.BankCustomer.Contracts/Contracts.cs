using System;

namespace Gringotts.BankCustomer.Contracts
{
    public record CustomerCreated(Guid CustomerId, string Name, string Surname, string Address, string PhoneNumber);

    public record CustomerUpdated(Guid CustomerId, string Name, string Surname, string Address, string PhoneNumber);

    public record CustomerDeleted(Guid CustomerId);
}