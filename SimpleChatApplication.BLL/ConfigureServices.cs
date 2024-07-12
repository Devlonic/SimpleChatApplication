using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexTube.Application.Common.Behaviours;
using SimpleChatApplication.BLL.Behaviors;
using SimpleChatApplication.BLL.CQRS.Events;
using SimpleChatApplication.BLL.Mappings;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.DAL.Data.Contexts;
using SimpleChatApplication.DAL.Data.UnitOfWorks;
using SimpleChatApplication.DAL.Entities;
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

            // add AutoMapper for automatic types mapping
            services.AddAutoMapper(config => {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                config.AddProfile(new AssemblyMappingProfile(typeof(ChatApplicationDbContext).Assembly));

            });

            services.AddSingleton<IChatService, RealTimeChatService>();

            return services;
        }
    }
}
