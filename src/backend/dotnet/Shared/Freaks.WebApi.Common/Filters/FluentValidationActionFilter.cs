using FluentValidation;
using Freaks.WebApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Freaks.WebApi.Common.Filters;

/// <summary>
///     Фильтр для автоматической валидации моделей запросов через FluentValidation.
/// </summary>
public class FluentValidationActionFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="FluentValidationActionFilter" />.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов для получения валидаторов.</param>
    public FluentValidationActionFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Проходим по всем аргументам действия
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value == null)
            {
                continue;
            }

            var argumentType = argument.Value.GetType();

            // Получаем тип валидатора для данного аргумента
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

            // Пытаемся получить валидатор из DI контейнера
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator == null)
            {
                continue;
            }

            // Создаем контекст валидации
            var validationContext = new ValidationContext<object>(argument.Value);

            // Выполняем валидацию
            var validationResult = await validator.ValidateAsync(validationContext);

            // Если есть ошибки, бросаем исключение
            if (validationResult.IsValid)
            {
                continue;
            }

            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            throw new ValidationApiException(errors, "Validation failed");
        }

        // Если валидация прошла успешно, продолжаем выполнение
        await next();
    }
}