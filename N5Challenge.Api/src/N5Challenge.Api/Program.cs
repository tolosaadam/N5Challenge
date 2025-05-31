using N5Challenge.Api.EndpointsDefinitions;
using N5Challenge.Api.Extensions;
using N5Challenge.Api.Infraestructure.SQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddMediatRSettings()
    .AddAutoMapperSettings()
    .AddValidatorSettings()
    .AddInfraestructureSettings(builder.Configuration);
//.AddConfiguration()
//.AddLoggingSettings(builder.Configuration, builder.Environment)
//.AddSQLSettings(builder.Configuration)
//.AddSwaggerSettings();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPermissionEndpoints()
    .Run();
