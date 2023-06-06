using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Generates specification json file for the reqeust
    app.UseSwaggerUI(); // Generates UI for that specifications and show it
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Below code output the stirng if there is no middleware configured
// remove "swagger" from the url
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});

app.Run();
