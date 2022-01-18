using System;
using Gringotts.Common;

namespace Gringotts.BankAccounts.Service.Entities
{
    public class Customer : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}