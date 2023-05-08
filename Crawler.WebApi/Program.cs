using Crawler.WebApp;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthorization();

app.Map("/error", webApp => webApp.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>();
    var statusCode = exception.Error is ArgumentException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)statusCode;

    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = exception.Error?.Message }));
}));

app.Run();
