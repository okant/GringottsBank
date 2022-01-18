using Gringotts.BankCustomer.Service.Dtos;
using Gringotts.BankCustomer.Service.Entities;

namespace Gringotts.BankCustomer.Service
{
    public static class Extensions
    {
        public static CustomerDto AsDto(this Customer customer)
        {
            return new CustomerDto(customer.Id, customer.Name, customer.Surname, customer.Address, customer.PhoneNumber, customer.CreatedDate);
        }
    }
}