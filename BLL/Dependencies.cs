using BLL.UserLogic.Handlers;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using JpProject.AspNetCore.PasswordHasher.Bcrypt;
using DAL;

namespace BLL
{
    public static class Dependencies
    {
        public static IServiceCollection RegisterBussinessLogicDependencies(
        this IServiceCollection services, IConfigurationManager config)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(config["Connection:DatabaseConnection"]!));

            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<ToDoItem>, ToDoItemRepository>();

            services.AddSingleton<IPasswordHasher<User>, BCrypt<User>>();

            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddRequestPostProcessor<PostRegisterRequestHandler>();
                cfg.AddRequestPostProcessor<PostLoginRequestHandler>();
            });


            return services;
        }
    }
}
