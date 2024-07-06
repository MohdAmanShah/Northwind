using Northwind.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Blazor.Services
{
    public interface INorthwindService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<List<Customer>> GetCustomersAsync(string country);
        Task<Customer?> GetCustomerAsync(string id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string id);
    }
}
