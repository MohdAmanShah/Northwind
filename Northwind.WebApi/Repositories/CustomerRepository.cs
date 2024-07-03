using Microsoft.EntityFrameworkCore.ChangeTracking; // To use EntityEntry<T>
using Northwind.EntityModels; // To use Customer
using Microsoft.Extensions.Caching.Memory; // To use IMemoryCache.
using Microsoft.EntityFrameworkCore;
using Northwind.Context; // To use ToArrayAsync().

namespace Northwind.WebApi.Repositories;
public class CustomerRepository : ICustomerRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(30)
    };
    private NorthwindDataContext _db;

    public CustomerRepository(NorthwindDataContext db, IMemoryCache memoryCache)
    {
        _db = db;
        _memoryCache = memoryCache;
    }

    public async Task<Customer?> CreateAsync(Customer customer)
    {
        customer.CustomerId = customer.CustomerId.ToUpper(); // Normalize to uppercase
        EntityEntry<Customer> added = await _db.Customers.AddAsync(customer);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            _memoryCache.Set(customer.CustomerId, customer, _cacheEntryOptions);
            return customer;
        }
        return null;
    }

    public Task<Customer[]> RetrieveAllAsync()
    {
        return _db.Customers.ToArrayAsync();
    }

    public Task<Customer?> RetrieveAsync(string id)
    {
        id = id.ToUpper();

        if (_memoryCache.TryGetValue(id, out Customer? fromCache))
            return Task.FromResult(fromCache);

        Customer? fromDb = _db.Customers.FirstOrDefault(c => c.CustomerId == id);

        if (fromDb is null) return Task.FromResult(fromDb);

        _memoryCache.Set(fromDb.CustomerId, fromDb, _cacheEntryOptions);
        return Task.FromResult(fromDb)!;
    }
    public async Task<Customer?> UpdateAsync(Customer customer)
    {
        customer.CustomerId = customer.CustomerId.ToUpper();

        _db.Customers.Update(customer);
        int affected = _db.SaveChanges();

        if (affected == 1)
        {
            _memoryCache.Set(customer.CustomerId, customer, _cacheEntryOptions);
            return customer;
        }
        return null;
    }

    public async Task<bool?> DeleteAsync(string id)
    {
        id = id.ToUpper();

        Customer? c = await _db.Customers.FindAsync(id);
        if (c is null) return null;

        _db.Customers.Remove(c);
        int affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            return true;
        }
        return null;
    }
}
