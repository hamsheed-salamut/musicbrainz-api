using Core.MusicBrainz.Application.Services;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Mssql;
using Infrastructure.Transport;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Serilog;
using WebApi.MusicBrainz.Health;
using WebApi.MusicBrainz.Requests;
using WebApi.MusicBrainz.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddHealthChecks().AddDbContextCheck<DataContext>(name: "Mssql Database");
builder.Services.AddMssql(builder.Configuration);
builder.Services.AddHttpTransport(builder.Configuration);
builder.Services.AddArtistService();
builder.Services.AddScoped<IValidator<ArtistRequest>, ArtistRequestValidator>();
builder.Services.AddScoped<IValidator<AlbumRequest>, AlbumRequestValidator>();

var app = builder.Build();
app.Lifetime.ApplicationStarted.Register(() => Log.Logger.Information($"ApplicationStarted - MachineName: {Environment.MachineName}"));
app.Lifetime.ApplicationStopped.Register(() => Log.Logger.Information($"ApplicationStopped - MachineName: {Environment.MachineName}"));
app.Lifetime.ApplicationStopping.Register(() => Log.Logger.Information($"ApplicationStopping - MachineName: {Environment.MachineName}"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new HealthCheckReponse
        {
            Status = report.Status.ToString(),
            HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description
            }),
            HealthCheckDuration = report.TotalDuration
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});

app.Run();
