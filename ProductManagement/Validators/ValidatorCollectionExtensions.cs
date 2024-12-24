using FluentValidation;
using ProductManagement.DTOs;

namespace ProductManagement.Validators
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
