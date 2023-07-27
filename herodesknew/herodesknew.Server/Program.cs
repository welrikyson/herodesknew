using herodesknew.Application.Attachments.Queries.GetAttachment;
using herodesknew.Application.Local;
using herodesknew.Application.PullRequests.Commands.CreatePullRequest;
using herodesknew.Application.PullRequests.Queries.GetPullRequests;
using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using herodesknew.Domain.AppSettingEntities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Data.Contexts;
using herodesknew.Infrastructure.Data.Migrators.PullRequest;
using herodesknew.Infrastructure.Data.Repositories;
using herodesknew.Server.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InfrastructureServiceInstall(builder.Configuration);
builder.Services.UtilsServiceInstall(builder.Configuration);

//TODO: Refactory service install 
builder.Services.AddTransient<GetFilteredTicketsQueryHandler>();
builder.Services.AddTransient<GetAttachmentQueryHandler>();
builder.Services.AddTransient<GetPullRequestsQueryHandler>();
builder.Services.AddTransient<CreatePullRequestCommandHandler>();
builder.Services.AddTransient<SqlExecutionPlanDoc>();

builder.Services.AddDbContext<HerodesknewDbContext>();
builder.Services.AddTransient<PullRequestDataMigrator>();
builder.Services.AddTransient<ISqlFileRepository, LocalSqlFileRepository>();

var azureReposSettings = builder.Configuration.GetSection("AzureReposSettings").Get<AzureReposSettings>()!;
builder.Services.AddSingleton(azureReposSettings);

//--------

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

var app = builder.Build();
app.UseCors();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
