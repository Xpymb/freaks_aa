using Microsoft.AspNetCore.Hosting;

namespace Freaks.WebApi.Common.Extensions;

/// <summary>
///     Расширения для работы с окружением (<see cref="IWebHostEnvironment" />).
///     Позволяют добавлять проверки на пользовательские названия окружений.
/// </summary>
public static class EnvironmentExtensions
{
    /// <summary>
    ///     Проверяет, запущено ли приложение в окружении с именем "Compose".
    ///     Используется, например, при запуске приложения внутри Docker Compose.
    /// </summary>
    /// <param name="environment">Объект окружения хоста.</param>
    /// <returns><c>true</c>, если текущее окружение называется "Compose"; иначе <c>false</c>.</returns>
    public static bool IsCompose(this IWebHostEnvironment environment)
    {
        return environment.EnvironmentName.Equals("Compose");
    }
}