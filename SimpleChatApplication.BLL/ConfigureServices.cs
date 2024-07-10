using Ardalis.GuardClauses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexTube.Application.Common.Behaviours;
using SimpleChatApplication.BLL.Behaviors;
using SimpleChatApplication.DAL.Data.Contexts;
using SimpleChatApplication.DAL.Data.UnitOfWorks;
using SimpleChatApplication.DAL.Interfaces;
using System.Reflection;

namespace SimpleChatApplication.BLL {
    public static class ConfigureServices {
        public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection services, IConfiguration configuration) {
            // find all custom validators, that inherit AbstractValidator
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(config => {
                // register all MediatR custom services
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                // each MediatR request before execution would be passed through pipes
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}
