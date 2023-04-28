using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Remove all the logging provider and add only specific logging provider
//builder.Logging.ClearProviders();
//builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// This service provides us the file extension types
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Generates specification json file for the reqeust
    app.UseSwaggerUI(); // Generates UI for that specifications and show it
}

app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthorization();

//app.UseEndpoints(endpoints => {
//    endpoints.MapControllers();
//});

app.MapControllers();

// Below code output the stirng if there is no middleware configured
// remove "swagger" from the url
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Hello world!");
//});

app.Run();
