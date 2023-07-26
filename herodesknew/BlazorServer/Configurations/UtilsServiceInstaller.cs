using herodesknew.Infrastructure.Data.Contexts;
using Scrutor;

namespace herodesknew.BlazorServer.Configurations
{
    public static class UtilsServiceInstaller
    {
        public static void UtilsServiceInstall(this IServiceCollection services, IConfiguration configuration)
        {
            services
                  .Scan(
                      selector => selector
                          .FromAssemblies(
                             Utils.AssemblyReference.Assembly)
                          .AddClasses(false)
                          .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                          .AsMatchingInterface()
                          .WithScopedLifetime());
                  
        }
    }
}
