using herodesknew.Application.Attachments.Queries.GetAttachment;
using herodesknew.Application.PullRequests.Commands.CreatePullRequest;
using herodesknew.Application.PullRequests.Queries.GetPullRequests;
using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using herodesknew.Application.Tickets.Queries.GetTicket;
using herodesknew.Domain.AppSettingEntities;
using herodesknew.Infrastructure.Data.Contexts;
using Scrutor;

namespace herodesknew.Server;

public static class ServiceInstaller
{
    public static void Install(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Scan(
                selector => selector
                    .FromAssemblies(
                        Infrastructure.AssemblyReference.Assembly)
                    .AddClasses(false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

        services.AddTransient<GetFilteredTicketsQueryHandler>();
        services.AddTransient<GetTicketQueryHandler>();
        services.AddTransient<GetAttachmentQueryHandler>();
        services.AddTransient<GetPullRequestsQueryHandler>();
        services.AddTransient<CreatePullRequestCommandHandler>();

        services.AddDbContext<HerodesknewDbContext>();
        services.AddTransient<HelpdeskContext>();

        services.AddSingleton(configuration.GetSection("AzureReposSettings").Get<AzureReposSettings>()!);

        services.AddCors(
            options => options
                .AddPolicy(
                    "AllowAllOrigins", 
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));
    }
}
