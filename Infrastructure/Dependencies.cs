using BLL;
using BLL.UserLogic.Commands;
using BLL.UserLogic.Handlers;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace DAL
{
    public static class Dependencies
    {
        public static IServiceCollection RegisterRequestHandlers(
        this IServiceCollection services)
        {
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                //cfg.AddRequestPostProcessor<IRequestPostProcessor<RegisterCommand, UserDTO>>();
                cfg.AddRequestPostProcessor<PostRegisterRequestHandler>();
                cfg.AddRequestPostProcessor<PostLoginRequestHandler>();
            });


            return services;
        }
    }
}
