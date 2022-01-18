using System.Threading.Tasks;
using Gringotts.BankAccounts.Service.Entities;
using Gringotts.BankCustomer.Contracts;
using Gringotts.Common;
using MassTransit;

namespace Gringotts.BankAccounts.Service.Consumers
{
    public class CustomerCreatedConsumer : IConsumer<CustomerCreated>
    {
        private readonly IRepository<Customer> repository;

        public CustomerCreatedConsumer(IRepository<Customer> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CustomerCreated> context)
        {
            var message = context.Message;

            var customer = await repository.GetAsync(message.CustomerId);

            if (customer != null)
            {
                return;
            }

            customer = new Customer
            {
                Id = message.CustomerId,
                Name = message.Name,
                Surname = message.Surname
            };

            await repository.CreateAsync(customer);
        }
    }
}