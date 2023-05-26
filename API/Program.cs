using System;
using API.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using Microsoft.AspNetCore.Http;

// Some variables required for building the web app and injection.
var builder = WebApplication.CreateBuilder(args);
string clientId = "Sh7J7L0YFbDywsABEY11uw";
string secret = "9__bIHJRvhH1sl6MlqSVYJJMXIwzyA";
string redirectUri = "http://localhost:5145/callback/redirect";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection. Whenever a controller requires the RedditOAuthService it is 
// injected automatically from here.
builder.Services.AddTransient<RedditOAuthService>(provider =>
{
    return new RedditOAuthService(clientId, secret, redirectUri);
});

// Services required for Cache usage. Not the best security measure, but
// it stores the access token as a cookie and the controllers are free to
// retrieve it from the cache whenever needed.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "YourSessionCookieName";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Services required for the View() methods in CallbackController.
builder.Services.AddTransient<RedditApiService>(provider => {
    return new RedditApiService(new HttpClient());
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
