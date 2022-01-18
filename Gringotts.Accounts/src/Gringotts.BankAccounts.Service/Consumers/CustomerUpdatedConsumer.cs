using System.Threading.Tasks;
using Gringotts.BankAccounts.Service.Entities;
using Gringotts.BankCustomer.Contracts;
using Gringotts.Common;
using MassTransit;

namespace Gringotts.BankAccounts.Service.Consumers
{
    public class CustomerUpdatedConsumer : IConsumer<CustomerUpdated>
    {
        private readonly IRepository<Customer> repository;

        public CustomerUpdatedConsumer(IRepository<Customer> repository)
        {
            this.repository = repository;
        }
        public async Task Consume(ConsumeContext<CustomerUpdated> context)
        {
            var message = context.Message;

            var customer = await repository.GetAsync(message.CustomerId);

            if (customer == null)
            {
                customer = new Customer
                {
                    Id = message.CustomerId,
                    Name = message.Name,
                    Surname = message.Surname
                };

                await repository.CreateAsync(customer);
            }
            else
            {
                customer.Name = message.Name;
                customer.Surname = message.Surname;

                await repository.UpdateAsync(customer);
            }
        }
    }
}