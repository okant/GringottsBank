using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Gringotts.BankAccounts.Service.Dtos;

namespace Gringotts.BankAccounts.Service.Clients
{
    public class CustomerClient
    {
        private readonly HttpClient httpClient;

        public CustomerClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<CustomerDto>> GetCustomersAsync()
        {
            var customers = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CustomerDto>>("/customers");

            return customers;
        }
    }
}