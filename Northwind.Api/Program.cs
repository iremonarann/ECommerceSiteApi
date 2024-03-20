using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Northwind.Api.Middlewares;
using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.Business.Data.Contexts;
using Northwind.Business.Exceptions;
using Northwind.Business.Profiles;
using Northwind.Business.Validators;
using Serilog;
using System.Security.Claims;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

//Asp.Net Core Provider Yönetimi
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryService, CategoryBusiness>();
builder.Services.AddScoped<IProductService, ProductBusiness>();
builder.Services.AddDbContext<NorthwindContext>(opt =>
{
    //opt.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["NorthwindConnection"]);
    //opt.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:NorthwindConnection"));

    opt.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnection"));
});

builder.Services.AddDataProtection();

builder.Services.AddAutoMapper(typeof(DtoToEntityProfile), typeof(EntityToDtoProfile));

builder.Services.AddFluentValidationAutoValidation(opt =>
{
    opt.DisableDataAnnotationsValidation = true;
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

builder.Services.AddProblemDetails(opt =>
{
    opt.IncludeExceptionDetails = (context, ex) => builder.Environment.IsDevelopment();
    opt.Map<ArgumentNullException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = ex.Message,
        Detail = "Null referans hatasý oluþtu",
        Status = 500,
        Type = nameof(ArgumentNullException),
    });

    opt.Map<BusinessRuleException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = "Ýþ Kuralý hatasý",
        Detail = ex.Message,
        Status = 400,
        Type = nameof(BusinessRuleException),
    });

    opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);

});


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        var key = builder.Configuration.GetValue<string>("Authentication:Jwt:SecretKey");
        var issuer = builder.Configuration.GetValue<string>("Authentication:Jwt:Issuer");

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,


            IssuerSigningKey = symmetricKey,
            ValidIssuer = issuer,
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("HasSpecialRule", builder =>
    {
        builder.RequireRole("admin");
        builder.RequireClaim(ClaimTypes.Email);
        builder.RequireUserName("salihdemirog");
    });

    opt.AddPolicy("HasGmailAddress", builder =>
    {
        builder.RequireAssertion(context =>
        {
            var mailClaim = context.User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Email);
            if (mailClaim is null)
                return false;

            return mailClaim.Value.EndsWith("gmail.com");
        });
    });
});

builder.Services.AddCors();

var app = builder.Build();

app.UseProblemDetails();

app.UseCors(opt =>
{
    opt.AllowAnyOrigin();
    opt.AllowAnyMethod();
    opt.AllowAnyHeader();
});

//app.UseHttpsRedirection();

app.UseReqResLogMiddleware();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsEnvironment("PreProd"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseUnprotectedAuthHeader();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
