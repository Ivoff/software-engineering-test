using ForumAggregator.Application.DependencyInjection;
using ForumAggregator.Infraestructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfraestructure();
builder.Services.AddApplication();
builder.Services.AddControllers();
// builder.Services.AddAuthentication()

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
