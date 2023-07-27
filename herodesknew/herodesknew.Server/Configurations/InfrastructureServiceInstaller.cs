using herodesknew.Infrastructure.Data.Contexts;
using Scrutor;

namespace herodesknew.Server.Configurations
{
    public static class InfrastructureServiceInstaller
    {
        public static void InfrastructureServiceInstall(this IServiceCollection services, IConfiguration configuration)
        {
            services
                  .Scan(
                      selector => selector
                          .FromAssemblies(
                             Infrastructure.AssemblyReference.Assembly)
                          .AddClasses(false)
                          .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                          .AsMatchingInterface()
                          .WithScopedLifetime())
                  .AddTransient<HelpdeskContext>();
        }
    }
}
