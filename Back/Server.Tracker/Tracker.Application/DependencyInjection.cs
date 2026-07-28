using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tracker.Application.Behaviors;

namespace Tracker.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
                configuration.AddOpenBehavior(typeof(TransactionBehavior<,>));
                configuration.AddOpenBehavior(typeof(PerformanceBehavior<,>));
            });

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(pair => services.AddScoped(pair.InterfaceType, pair.ValidatorType));

            return services;
        }

    }
}
