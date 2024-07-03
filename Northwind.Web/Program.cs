using Northwind.Context; // To use NorthwindDataContext.

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Configure web server host and services.

builder.Services.AddRazorPages();
builder.Services.AddNorthwindDataContext();

#endregion

WebApplication app = builder.Build();

#region Configure the Http pipline and routes.

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();
app.MapGet("/Hello", () => $"Environment is: {app.Environment.EnvironmentName}");

#endregion

// Start the web server, host the website, and wait for requests.
app.Run(); // This is a thread-blocking call.
WriteLine("This executes after the web server has stopped!");
