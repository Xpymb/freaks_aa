using System.Reflection;
using FluentValidation;
using Freaks.WebApi.Common.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.WebApi.Common.Extensions;

public static class ValidationExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);

        // Регистрируем фильтр для валидации в контроллерах
        services.AddScoped<FluentValidationActionFilter>();

        // Добавляем фильтр глобально к контроллерам через MvcOptions
        services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<FluentValidationActionFilter>();
        });

        return services;
    }
}