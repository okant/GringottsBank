using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gringotts.BankAccounts.Service.Dtos;
using Gringotts.BankAccounts.Service.Entities;
using Gringotts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gringotts.BankAccounts.Service.Controllers
{
    [ApiController]
    [Route("accounts")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IRepository<Account> accountsRepository;
        private readonly IRepository<Customer> customersRepository;
        private readonly ILogger<AccountsController> logger;

        public AccountsController(IRepository<Account> accountsRepository, IRepository<Customer> customersRepository, ILogger<AccountsController> logger)
        {
            this.accountsRepository = accountsRepository;
            this.customersRepository = customersRepository;
            this.logger = logger;
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<AccountDto>>> GetAsync(Guid customerId)
        // {
        //     if (customerId == Guid.Empty) return BadRequest();

        //     var accountEntities = await accountsRepository.GetAllAsync(account => account.CustomerId == customerId);

        //     var customerIds = accountEntities.Select(customer => customer.CustomerId);

        //     var customerEntities = await customersRepository.GetAllAsync(customer => customerIds.Contains(customer.Id));

        //     var accountDtos = accountEntities.Select(account =>
        //     {
        //         var customer = customerEntities.Single(customer => customer.Id == account.CustomerId);
        //         return account.AsDto(customer.Name, customer.Surname);
        //     });

        //     return Ok(accountDtos);
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest();

            var accountEntities = await accountsRepository.GetAllAsync(account => account.UserId == userId);

            var customerIds = accountEntities.Select(customer => customer.CustomerId);

            var customerEntities = await customersRepository.GetAllAsync(customer => customerIds.Contains(customer.Id));

            var accountDtos = accountEntities.Select(account =>
            {
                var customer = customerEntities.Single(customer => customer.Id == account.CustomerId);
                return account.AsDto(customer.Name, customer.Surname);
            });

            return Ok(accountDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CreateAccountDto createAccountDto)
        {
            var accountItem = await accountsRepository.GetAsync(
                account => account.UserId == createAccountDto.UserId && account.CustomerId == createAccountDto.CustomerId);

            if (accountItem == null)
            {
                accountItem = new Account
                {
                    AccountBalance = createAccountDto.AccountBalance,
                    AccountDescription = createAccountDto.AccountDescription,
                    AccountNo = createAccountDto.AccountNo,
                    AccountOpenDate = DateTimeOffset.UtcNow,
                    Currency = createAccountDto.Currency,
                    CustomerId = createAccountDto.CustomerId,
                    UserId = createAccountDto.UserId
                };

                await accountsRepository.CreateAsync(accountItem);
            }
            else
            {
                accountItem.AccountBalance += createAccountDto.AccountBalance;

                await accountsRepository.UpdateAsync(accountItem);
            }

            return Ok();
        }


        [HttpPost]
        [Route("addmoney")]
        public async Task<ActionResult> AddAsync(TransactionItem transactionItem)
        {
            var accountItem = await accountsRepository.GetAsync(
                account => account.CustomerId == transactionItem.CustomerId && account.Id == transactionItem.AccountId);

            if (accountItem == null)
            {
                return NotFound("No such an account");
            }
            else
            {
                accountItem.AccountBalance += transactionItem.Quantity;

                await accountsRepository.UpdateAsync(accountItem);
            }

            return Ok();
        }

        [HttpPost]
        [Route("withdrawmoney")]
        public async Task<ActionResult> WithdrawAsync(TransactionItem transactionItem)
        {
            var accountItem = await accountsRepository.GetAsync(
                account => account.CustomerId == transactionItem.CustomerId && account.Id == transactionItem.AccountId);

            if (accountItem == null)
            {
                return NotFound("No such an account");
            }
            else
            {
                if (accountItem.AccountBalance - transactionItem.Quantity < 0)
                {
                    return BadRequest("No sufficient balance in your account");
                    //throw new InvalidOperationException("No sufficient balance in your account");                    
                }
                else
                    accountItem.AccountBalance -= transactionItem.Quantity;

                await accountsRepository.UpdateAsync(accountItem);
            }

            return Ok();
        }
    }
}