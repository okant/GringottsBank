using System;
using System.ComponentModel.DataAnnotations;

namespace Gringotts.BankCustomer.Service.Dtos
{
    public record CustomerDto(Guid Id, string Name, string Surname, string Address, string PhoneNumber, DateTimeOffset CreatedDate);

    public record CreateCustomerDto([Required] string Name, string Surname, string Address, string PhoneNumber);

    public record UpdateCustomerDto([Required] string Name, string Surname, string Address, string PhoneNumber);
}