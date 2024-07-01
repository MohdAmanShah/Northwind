var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

#region Configure the Http pipline and routes.

if(!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

#endregion
app.UseHttpsRedirection();


app.MapGet("/", () => "Hello World!");

// Start the web server, host the website, and wait for requests.
app.Run(); // This is a thread-blocking call.
WriteLine("This executes after the web server has stopped!");
