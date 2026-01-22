using System.Reflection;
using FluentValidation;
using Freaks.WebApi.Common.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Freaks.WebApi.Common.Extensions;

/// <summary>
///     Методы расширения для настройки FluentValidation в приложении.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    ///     Добавляет FluentValidation в сервисы приложения.
    ///     Регистрирует все валидаторы из указанной сборки и настраивает глобальный фильтр для автоматической валидации запросов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="assembly">Сборка, из которой нужно зарегистрировать валидаторы.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
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