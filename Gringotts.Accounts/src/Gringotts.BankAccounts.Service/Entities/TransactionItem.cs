using System;

namespace Gringotts.BankAccounts.Service.Entities
{
    public class TransactionItem
    {
        public Guid AccountId { get; set; }

        public Guid CustomerId { get; set; }

        public decimal Quantity { get; set; }
    }
}