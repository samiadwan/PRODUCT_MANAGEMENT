using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ProductManagement.DTOs;
using ProductManagement.Validators;

namespace ProductManagement.Services
{
    public static class ValidatorCollectionExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddScoped<IValidator<UserDto>, UserValidator>();
            services.AddScoped<IValidator<AddressDTO>, AddressValidator>();
            services.AddScoped<IValidator<ProductDto>, ProductValidator>();
            services.AddScoped<IValidator<OrderDto>, OrderValidator>();
            services.AddScoped<IValidator<OrderItemDto>, OrderItemValidator>();

            return services;
        }
    }
}
