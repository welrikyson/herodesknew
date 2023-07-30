using herodesknew.Application.Attachments.Queries.GetAttachment;
using herodesknew.Application.PullRequests.Commands.CreatePullRequest;
using herodesknew.Application.PullRequests.Queries.GetPullRequests;
using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using herodesknew.Domain.AppSettingEntities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Data.Contexts;
using herodesknew.Infrastructure.Data.Repositories;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .Scan(
        selector => selector
            .FromAssemblies(
                herodesknew.Infrastructure.AssemblyReference.Assembly,
                 herodesknew.Local.Domain.AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

#region services install
//TODO: Refactory service install 
builder.Services.AddTransient<GetFilteredTicketsQueryHandler>();
builder.Services.AddTransient<GetAttachmentQueryHandler>();
builder.Services.AddTransient<GetPullRequestsQueryHandler>();
builder.Services.AddTransient<CreatePullRequestCommandHandler>();
builder.Services.AddTransient<herodesknew.Local.Application.Tickets.Queries.GetTickets.GetTicketsQueryHandler>();


builder.Services.AddDbContext<HerodesknewDbContext>();
builder.Services.AddTransient<HelpdeskContext>();

var azureReposSettings = builder.Configuration.GetSection("AzureReposSettings").Get<AzureReposSettings>()!;
builder.Services.AddSingleton(azureReposSettings);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
