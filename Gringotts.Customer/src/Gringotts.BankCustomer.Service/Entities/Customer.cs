using System;
using Gringotts.Common;

namespace Gringotts.BankCustomer.Service.Entities
{
    public class Customer : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}