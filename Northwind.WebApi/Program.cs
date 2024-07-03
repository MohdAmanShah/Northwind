using Microsoft.Extensions.Caching.Memory; // to use IMemoryCache
using Microsoft.AspNetCore.Mvc.Formatters;
using Northwind.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
builder.Services.AddControllers(opt =>
{
    Console.WriteLine("Default output formatters.");
    foreach (IOutputFormatter formatter in opt.OutputFormatters)
    {
        OutputFormatter fter = formatter as OutputFormatter;
        if (fter is null)
        {
            Console.WriteLine($"   {formatter.GetType().Name}");
        }
        else
        {
            Console.WriteLine($"   {fter.GetType().Name}, Media Types: {String.Join(",", fter.SupportedMediaTypes)}");
        }
    }
})
    .AddXmlDataContractSerializerFormatters()
    .AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNorthwindDataContext();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
