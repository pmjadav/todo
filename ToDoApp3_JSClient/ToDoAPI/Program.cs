using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoAPI.Data;
using ToDoAPI.Extensions;
using Microsoft.AspNetCore.HttpOverrides; // forwarded headers

var builder = WebApplication.CreateBuilder(args);
/* Creates a WebApplicationBuilder object. Using this class we can:
 * Add Configuration to the project by using the builder.Configuration property
 * Register services in our app with the builder.Services property
 * Log configuration with the builder.Logging property
 * Other IHostBuilder and IWebHostBuilder configuration
 */

// Add services to the container.

builder.Services.ConfigureCors(); // My extension method to configure CORS
builder.Services.ConfigureIISIntegration(); // My extention method for IIS integration

builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoContext") 
            ?? throw new InvalidOperationException("Connection string 'ToDoContext' not found.")));
//builder.Services.AddDbContext<ToDoContext>(options =>
//    options.UseInMemoryDatabase("TodoList"));

builder.Services.AddControllers(); // registers only the controllers in IServiceCollection
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();  // Create WebApplication object
/* WebApplication implements 
 *  IHost to start and stop the host, 
 *  IApplicationBuilder to build the middleware pipeline 
 *  IEndpointRouteBuilder to add endpoints in our app
 */

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts(); // Adds HTTP Strict Transport Security Protocol (HSTS) header
}

app.UseHttpsRedirection();  // redirection from HTTP to HTTPS
app.UseDefaultFiles();      // URL Rewriter: search wwwroot for default.htm/l, index.htm/l
app.UseStaticFiles();       // serve files outside the web root (wwwroot)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
}); // forward proxy headers to the current request

app.UseCors("CorsPolicy");

app.UseAuthorization(); 
// adds the authorization middleware to the specified IApplicationBuilder to enable authorization capabilities

app.MapControllers();

app.Run();
