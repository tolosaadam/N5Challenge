using N5Challenge.Api.EndpointsDefinitions;
using N5Challenge.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddMediatRSettings()
    .AddAutoMapperSettings()
    .AddValidatorSettings();
//.AddConfiguration()
//.AddLoggingSettings(builder.Configuration, builder.Environment)
//.AddSQLSettings(builder.Configuration)
//.AddSwaggerSettings();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPermissionEndpoints()
    .Run();
