using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskIt.API.Core.Middleware;
using TaskIt.API.Extensions;
using TaskIt.API.Models;
using TaskIt.API.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new Exception("Exception: Connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddIdentity<ApiUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
    };
});


builder.Services.AddControllers(options =>
{
    // Default model binding error messages are modified here.
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
        (x) => $"The value {x} is invalid.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        (x) => $"The value {x} must be a number.");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (x, y) => $"The value {x} is not valid for {y}.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
        () => $"A value is required.");

    // Caching profiles to prevent having to repeat caching directives on multiple action methods with identical caching behavior.
    // Example usage: [ResponseCache(CacheProfileName = "Any-60")]
    options.CacheProfiles.Add("NoCache", new CacheProfile()
    {
        NoStore = true
    });

    options.CacheProfiles.Add("Any-60", new CacheProfile()
    {
        Location = ResponseCacheLocation.Any,
        Duration = 60
    });

})
.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(cfg => {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]!);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
    options.AddPolicy(name: "AnyOrigin", cfg => {
        cfg.AllowAnyOrigin();
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
});


// Middleware to add server-side response caching (must also include app.UseResponseCaching() after app.UseCors() below).
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 32 * 1024 * 1024;
    options.SizeLimit = 50 * 1024 * 1024;
    options.UseCaseSensitivePaths = false;
});


// Middleware to add in-memory caching.
builder.Services.AddMemoryCache();


builder.Services.AddScoped<ErrorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.

// bool values described below are values in appsettings.json files.
if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Send all relevant error info to a customizable handler instead of presenting it to the end user.
    app.UseExceptionHandler("/error");
}


app.UseHttpsRedirection();


app.UseCors();


app.UseResponseCaching();


app.UseAuthentication();


app.UseAuthorization();


app.Use((context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
    {
        NoCache = true,
        NoStore = true
    };
    return next.Invoke();
});


// Global exception handling
//app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExtensionHandler());


// Minimal API routing
app.MapGet("/error",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () => Results.Problem());

app.MapGet("/error/test",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () => { throw new Exception("test"); });

app.MapGet("/cod/test",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] 
    () => Results.Text("<script>window.alert('Your client supports JavaScript!\\r\\n\\r\\n" +
                       $"Server time (UTC): {DateTime.UtcNow.ToString("o")}\\r\\n" +
                       "Client time (UTC): ' + new Date().toISOString());</script>" +
                       "<noscript>Your client does not support JavaScript</noscript>",
                       "text/html"));


app.MapControllers()
    .RequireCors("AnyOrigin");

app.Run();
