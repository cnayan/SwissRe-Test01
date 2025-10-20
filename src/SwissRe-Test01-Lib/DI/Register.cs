using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace SwissRe_Test01_Lib.DI;

[ExcludeFromCodeCoverage]
public static class Register
{
    public static IServiceCollection AddSwissReTest01Lib(this IServiceCollection services)
    {
        services.AddScoped<IManagerReporter, ManagerReporter>();
        return services;
    }
}
