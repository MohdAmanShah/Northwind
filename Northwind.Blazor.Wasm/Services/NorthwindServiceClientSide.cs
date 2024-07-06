using Northwind.Blazor.Services;
using System.Net.Http.Json;// To use GetFromJsonAsync()
using Microsoft.Extensions.Http;
using Northwind.EntityModels;

namespace Northwind.Blazor.Wasm.Services;
public class NorthwindServicesClientSide : INorthwindService
{
    private readonly IHttpClientFactory _clientFactory;
    public NorthwindServicesClientSide(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }
    public Task<List<Customer>> GetCustomersAsync()
    {
        HttpClient client = _clientFactory.CreateClient(
          name: "Northwind.WebApi");

        return client.GetFromJsonAsync
          <List<Customer>>("api/customer")!;
    }

    public Task<List<Customer>> GetCustomersAsync(string country)
    {
        HttpClient client = _clientFactory.CreateClient(
          name: "Northwind.WebApi");

        return client.GetFromJsonAsync
          <List<Customer>>($"api/customer/in/{country}")!;
    }

    public Task<Customer?> GetCustomerAsync(string id)
    {
        HttpClient client = _clientFactory.CreateClient(
          name: "Northwind.WebApi");

        return client.GetFromJsonAsync
          <Customer>($"api/customer/{id}");
    }

    public async Task<Customer>
      CreateCustomerAsync(Customer c)
    {
        HttpClient client = _clientFactory.CreateClient(
          name: "Northwind.WebApi");

        HttpResponseMessage response = await
          client.PostAsJsonAsync("api/customer", c);

        return (await response.Content
          .ReadFromJsonAsync<Customer>())!;
    }

    public async Task<Customer> UpdateCustomerAsync(Customer c)
    {
        HttpClient client = _clientFactory.CreateClient(
          name: "Northwind.WebApi");

        HttpResponseMessage response = await
          client.PutAsJsonAsync("api/customer", c);

        return (await response.Content
          .ReadFromJsonAsync<Customer>())!;
    }

    public async Task DeleteCustomerAsync(string id)
    {
        HttpClient client = _clientFactory.CreateClient(
          name: "Northwind.WebApi");

        HttpResponseMessage response = await
          client.DeleteAsync($"api/customer/{id}");
    }
}
