using herodesknew.Application.Attachments.Queries.GetAttachment;
using herodesknew.Application.PullRequests.Commands.CreatePullRequest;
using herodesknew.Application.PullRequests.Queries.GetPullRequests;
using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using herodesknew.Application.Tickets.Queries.GetTicket;
using herodesknew.Domain.AppSettingEntities;
using herodesknew.Infrastructure.Data.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Cli
{
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
            
        }
    }

}
