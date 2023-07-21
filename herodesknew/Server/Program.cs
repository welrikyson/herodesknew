using Microsoft.AspNetCore.ResponseCompression;
using herodesknew.Server.Configurations;
using herodesknew.Application.Tickets.Queries.GetTickets;
using herodesknew.Application.Attachments.Queries.GetAttachment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.InfrastructureServiceInstall(builder.Configuration);

//TODO: Refactory service install 
builder.Services.AddTransient<GetMembersQueryHandler>();
builder.Services.AddTransient<GetAttachmentQueryHandler>();
//--------

builder.Services.AddOpenApiDocument( configure => configure.Title = "herodesknew api");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();


app.UseOpenApi();
app.UseSwaggerUi3();
app.UseReDoc(configure => configure.Path = "/redoc");

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

