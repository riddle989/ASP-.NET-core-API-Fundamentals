using CityInfo.API;
using CityInfo.API.DbContexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

// Configure the logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Remove all the logging provider and add only specific logging provider
//builder.Logging.ClearProviders();
//builder.Logging.AddDebug();

// Add 3rd party logger service
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

/* Built-in services, that exposes information of API, like available endpoints / how to interact with them */
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth"
                }
            }, new List<string>()
        }
    });
});

// This service provides us the file extension types
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();


/*====================Start Custom services====================*/

/* Use different service based on environment using compiler directive*/
#if DEBUG
/* This means, Whenever we inject "IMailService" in our code,we want to provide an instance of "LocalMailService" */
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<CityInfoContext>(
    dbContextOptions => dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]
        )
    );

/* "AddScoped" - created once per request */
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeSylhet", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Sylhet");
    });
});

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});





/*====================End Custom services====================*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /* This generates open api specification in json format, based on this A UI is created*/
    app.UseSwagger(); // Generates specification json file for the reqeust

    /* Creates a UI, based on specification */
    app.UseSwaggerUI(); // Generates UI for that specifications and show it
}

app.UseHttpsRedirection();

//app.UseRouting();

/* Before any endpoint is selected and before checking any authorization policy we need to authenticate the endpoint, 
 So we configured our Authentication middleware 
*/
app.UseAuthentication();

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
