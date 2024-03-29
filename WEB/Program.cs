using BLL.Middlewares;
using BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Web.ExceptionHandlers;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true)
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });


            builder.Services.RegisterBussinessLogicDependencies(config);
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddExceptionHandler<UnauthorizedExceptionHandler>();
            builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
            builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            
            builder.Services.AddScoped<TokenValidationMiddleware>();
            
            
            builder.Services.AddProblemDetails();
            
            builder.Services.AddControllers();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidIssuer = config["JwtToken:Issuer"]!,
                        ValidateAudience = true,
                        ValidAudience = config["JwtToken:Audience"]!,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtToken:SecretKey"]!)),
                        ValidateIssuerSigningKey = true
                    };
                });



            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDoApi", Version = "v1" });
                swagger.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearerAuth"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            var app = builder.Build();

            app.UseExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            
            app.UseAuthorization();

            app.UseMiddleware<TokenValidationMiddleware>();
            

            app.MapControllers();

            app.Run();
        }
    }
}