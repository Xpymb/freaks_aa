using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Freaks.Files.SharedContracts.Requests;

/// <summary>
///     DTO-модель формы для загрузки файла через multipart/form-data.
///     Используется в HTTP-запросах с телом, содержащим файл.
/// </summary>
public class FileUploadForm
{
    /// <summary>
    ///     Загружаемый файл (например, изображение, документ и т.д.).
    ///     Должен быть передан как часть формы с типом <c>multipart/form-data</c>.
    /// </summary>
    [Required]
    public IFormFile File { get; set; } = default!;
}