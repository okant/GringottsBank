using System;
using Gringotts.Common;

namespace Gringotts.BankAccounts.Service.Entities
{
    public class Account : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CustomerId { get; set; }

        public string AccountNo { get; set; }

        public string AccountDescription { get; set; }

        public decimal AccountBalance { get; set; }

        public string Currency { get; set; }

        public DateTimeOffset AccountOpenDate { get; set; }
    }
}