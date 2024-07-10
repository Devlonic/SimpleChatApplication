using SimpleChatApplication.DAL;
using SimpleChatApplication.BLL;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DAL services
builder.Services.AddDataAccessLayerServices(configuration);

// Add BLL services
builder.Services.AddBusinessLogicLayerServices(configuration);

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

app.Run();
