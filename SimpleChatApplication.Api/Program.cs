using SimpleChatApplication.DAL;
using SimpleChatApplication.BLL;
using SimpleChatApplication.DAL.Data;
using SimpleChatApplication.Api.Hubs;
using SimpleChatApplication.BLL.CQRS.Events;
using SimpleChatApplication.BLL.Models.EventTypes;
using SimpleChatApplication.Api.Events;
using Microsoft.AspNetCore.SignalR;
using SimpleChatApplication.Api;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.Api.Services;
public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // setup SignalR
        builder.Services.AddSignalR(options => {
            if ( builder.Environment.IsDevelopment() ) {
                options.EnableDetailedErrors = true;
            }
        });

        // Add DAL services
        builder.Services.AddDataAccessLayerServices(configuration);

        // Add BLL services
        builder.Services.AddBusinessLogicLayerServices(configuration);

        // Add API services
        builder.Services.AddSingleton<IRealTimeMessenger, SignalrRealTimeMessenger>();

        // add SignalR wrappers
        builder.Services.AddScoped<IEventPublisher<ChatMessageEvent>, ChatMessageEventPublisher>();
        builder.Services.AddSingleton<IUserIdProvider, ApplicationUserIdProvider>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if ( app.Environment.IsDevelopment() ) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<ChatHub>("/chat");

        app.SeedData();

        app.Run();

    }
}
