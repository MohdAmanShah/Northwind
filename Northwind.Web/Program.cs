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

app.Use(async (HttpContext context, Func<Task> next) =>
{
    RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;

    if (rep is not null)
    {
        WriteLine($"Enpoint name: {rep.DisplayName}");
        WriteLine($"Endpoint route pattern: {rep.RoutePattern.RawText}");
        WriteLine(context.Request.Path);
        if(context.Request.Path == "/order")
        {
            context.Request.Path = "/suppliers";
        }
    }

    if (context.Request.Path == "/bonjour")
    {
        await context.Response.WriteAsync("BonjourModel");
        return;
    }

    await next();

});
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();
app.MapGet("/Hello", () => $"Environment is: {app.Environment.EnvironmentName}");
#endregion

// Start the web server, host the website, and wait for requests.
app.Run(); // This is a thread-blocking call.
WriteLine("This executes after the web server has stopped!");
