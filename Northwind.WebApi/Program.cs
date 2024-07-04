using Microsoft.Extensions.Caching.Memory; // to use IMemoryCache
using Microsoft.AspNetCore.Mvc.Formatters; // To IOutputFormatter
using Northwind.Context; // To use AddNorthwindDataContext extension method
using Northwind.WebApi.Repositories; // To use ICustomerRepository, CustomerRepository
using Swashbuckle.AspNetCore.SwaggerUI; // To configure UseSwaggerUI method
using Microsoft.AspNetCore.HttpLogging; // To use AddHttpLogging, UseHttpLogging

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4080;
    options.ResponseBodyLogLimit = 4080;
});
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
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind Service API Version 1.");

        c.SupportedSubmitMethods(new[]
        {
            SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete
        });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
