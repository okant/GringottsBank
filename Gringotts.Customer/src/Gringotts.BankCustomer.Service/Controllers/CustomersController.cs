using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gringotts.BankCustomer.Contracts;
using Gringotts.BankCustomer.Service.Dtos;
using Gringotts.BankCustomer.Service.Entities;
using Gringotts.Common;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gringotts.BankCustomer.Service.Controllers
{
    [ApiController]
    [Route("customers")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> customersRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public CustomersController(IRepository<Customer> customersRepository, IPublishEndpoint publishEndpoint)
        {
            this.customersRepository = customersRepository;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAsync()
        {
            var customers = (await customersRepository.GetAllAsync())
                        .Select(customer => customer.AsDto());

            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ActionName("GetByIdAsync")]
        public async Task<ActionResult<CustomerDto>> GetByIdAsync(Guid id)
        {
            var customer = await customersRepository.GetAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = new Customer
            {
                Name = createCustomerDto.Name,
                Surname = createCustomerDto.Surname,
                Address = createCustomerDto.Address,
                PhoneNumber = createCustomerDto.PhoneNumber,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await customersRepository.CreateAsync(customer);

            await publishEndpoint.Publish(new CustomerCreated(customer.Id, customer.Name, customer.Surname, customer.Address, customer.PhoneNumber));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateCustomerDto updateCustomerDto)
        {
            var existingCustomer = await customersRepository.GetAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.Name = updateCustomerDto.Name;
            existingCustomer.Surname = updateCustomerDto.Surname;
            existingCustomer.Address = updateCustomerDto.Address;
            existingCustomer.PhoneNumber = updateCustomerDto.PhoneNumber;

            await customersRepository.UpdateAsync(existingCustomer);

            await publishEndpoint.Publish(new CustomerUpdated(existingCustomer.Id, existingCustomer.Name, existingCustomer.Surname, existingCustomer.Address, existingCustomer.PhoneNumber));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var customer = await customersRepository.GetAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            await customersRepository.RemoveAsync(customer.Id);

            await publishEndpoint.Publish(new CustomerDeleted(customer.Id));

            return NoContent();
        }
    }
}