using herodesknew.Application.Attachments.Queries.GetAttachment;
using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using herodesknew.BlazorServer.Configurations;
using herodesknew.BlazorServer.Data;
using herodesknew.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.InfrastructureServiceInstall(builder.Configuration);

//TODO: Refactory service install 
builder.Services.AddTransient<GetFilteredTicketsQueryHandler>();
builder.Services.AddTransient<GetAttachmentQueryHandler>();
builder.Services.AddDbContext<HerodesknewDbContext>();
//--------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
