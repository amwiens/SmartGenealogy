using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;

using SmartGenealogy.Services;

namespace SmartGenealogy.Helpers;

/// <summary>
/// Registers services using <see cref="TransientAttribute"/> and <see cref="SingletonAttribute"/> attributes.
/// </summary>
[Localizable(false)]
internal static partial class AttributeServiceInjector
{

    public static IServiceCollection AddServicesByAttributes(this IServiceCollection services)
    {
#if REGISTER_SERVICE_REFLECTION
        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("SmartGenealogy") == true);
        AddServicesByAttributesReflection(services, assemblies);
#else
        AddServicesByAttributesSourceGen(services);
#endif
        return services;
    }

    /// <summary>
    /// Registers services from the assemblies using Attributes via source generation.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the services will be registered.
    /// </param>
    public static void AddServicesByAttributesSourceGen(IServiceCollection services)
    {
        services.AddSmartGenealogyCore();
        services.AddSmartGenealogy();
    }


    public static IServiceCollection AddServiceManagerWithCurrentCollectionServices<TService>(
        this IServiceCollection services,
        Func<ServiceDescriptor, bool>? serviceFilter = null)
    {
        return services.AddSingleton<ServiceManager<TService>>(provider =>
        {
            //using var _ = CodeTimer.StartDebug(
            //    callerName: $"{nameof(AddServiceManagerWithCurrentCollectionServies)}<{typeof(TService)}>");

            var serviceManager = new ServiceManager<TService>();

            // Get registered services that are assignable to TService
            var serviceDescriptors = services.Where(s => s.ServiceType.IsAssignableTo(typeof(TService)));

            // Optional filter
            if (serviceFilter is not null)
            {
                serviceDescriptors = serviceDescriptors.Where(serviceFilter);
            }

            foreach (var service in serviceDescriptors)
            {
                var type = service.ServiceType;
                Debug.Assert(type is not null, "type is not null");
                Debug.Assert(type.IsAssignableTo(typeof(TService)), "type is assignable to TService");

                //serviceManager.Register(type, () => (TService)provider.GetRequiredService(type));
            }

            return serviceManager;
        });
    }
}