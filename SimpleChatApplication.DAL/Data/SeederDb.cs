using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleChatApplication.DAL.Data.Contexts;

namespace SimpleChatApplication.DAL.Data {
    public static class SeederDB {
        public static void SeedData(this IApplicationBuilder app) {
            using ( var scope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope() ) {

                scope.ServiceProvider
                    .GetRequiredService<ChatApplicationDbContext>().Database.Migrate();
            }
        }
    }
}
