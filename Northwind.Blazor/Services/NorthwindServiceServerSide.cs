using Microsoft.EntityFrameworkCore;
using Northwind.Context;
using Northwind.EntityModels;

namespace Northwind.Blazor.Services
{
    public class NorthwindServiceServerSide : INorthwindService
    {
        private readonly NorthwindDataContext _db;

        public NorthwindServiceServerSide(NorthwindDataContext db)
        {
            _db = db;
        }

        public Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _db.Customers.Add(customer);
            _db.SaveChangesAsync();
            return Task.FromResult(customer);
        }

        public Task DeleteCustomerAsync(string id)
        {
            Customer? customer = _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id).Result;
            if (customer is null)
            {
                return Task.CompletedTask;
            }
            else
            {
                _db.Customers.Remove(customer);
                return _db.SaveChangesAsync();
            }
        }

        public Task<Customer?> GetCustomerAsync(string id)
        {
            return _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public Task<List<Customer>> GetCustomersAsync()
        {
            return _db.Customers.ToListAsync();
        }

        public Task<List<Customer>> GetCustomersAsync(string country)
        {
            return _db.Customers.Where(c => c.Country == country).ToListAsync();
            //var query = from customer in _db.Customers
            //            where customer.Country == country
            //            select customer;
            //return query.ToListAsync();
        }

        public Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            _db.Entry(customer).State = EntityState.Modified;
            _db.SaveChangesAsync();
            return Task.FromResult(customer);
        }
    }
}
