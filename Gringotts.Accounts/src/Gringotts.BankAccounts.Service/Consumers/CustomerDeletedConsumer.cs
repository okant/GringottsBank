using System.Threading.Tasks;
using Gringotts.BankAccounts.Service.Entities;
using Gringotts.BankCustomer.Contracts;
using Gringotts.Common;
using MassTransit;

namespace Gringotts.BankAccounts.Service.Consumers
{
    public class CustomerDeletedConsumer : IConsumer<CustomerDeleted>
    {
        private readonly IRepository<Customer> repository;

        public CustomerDeletedConsumer(IRepository<Customer> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CustomerDeleted> context)
        {
            var message = context.Message;

            var customer = await repository.GetAsync(message.CustomerId);

            if (customer == null)
            {
                return;
            }
            else
            {
                await repository.RemoveAsync(message.CustomerId);
            }
        }
    }
}