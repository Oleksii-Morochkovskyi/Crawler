using Crawler.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration connectionConfiguration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.ConfigureServices(connectionConfiguration);

var app = builder.Build();

//app.UseDefaultFiles();

//app.UseStaticFiles();
//app.Environment.EnvironmentName = "Production";

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();
