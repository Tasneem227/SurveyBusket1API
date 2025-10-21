using SurveyBusket8.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Identity setup
builder.Services.AddControllers();
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddDependencies(builder.Configuration);

//var config =builder.Configuration["MyKey"];

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.MapIdentityApi<ApplicationUser>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
