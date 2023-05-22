using System;
using API.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
string clientId = "Sh7J7L0YFbDywsABEY11uw";
string secret = "9__bIHJRvhH1sl6MlqSVYJJMXIwzyA";
string redirectUri = "http://localhost:5145/callback/redirect";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<RedditOAuthService>(provider =>
{
    return new RedditOAuthService(clientId, secret, redirectUri);
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "YourSessionCookieName";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
builder.Services.AddMvc().AddCookieTempDataProvider();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
