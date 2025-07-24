using Freaks.Files.Bll.Interfaces;
using Freaks.Files.SharedContracts.Dto;
using Freaks.Files.SharedContracts.Requests;
using Freaks.Files.SharedContracts.ValueObjects;
using Freaks.Users.Attributes;
using Freaks.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Files.WebApi.Controllers;

/// <summary>
///     Контроллер для работы с файлами, связанными с рейдами (например, скриншотами).
///     Позволяет загружать файлы определённого типа для конкретного рейда.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("raids/{raidId:int}")]
public class RaidFileController : ControllerBase
{
    private readonly IFileServiceFactory _serviceFactory;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера <see cref="RaidFileController"/>.
    /// </summary>
    /// <param name="serviceFactory">Фабрика сервисов, предоставляющая реализацию для работы с файлами разных типов.</param>
    public RaidFileController(IFileServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory ?? throw new ArgumentNullException(nameof(serviceFactory));
    }

    /// <summary>
    ///     Загружает файл, связанный с конкретным рейдом.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда, к которому относится файл.</param>
    /// <param name="fileType">Тип загружаемого файла (например, скриншот).</param>
    /// <param name="fileForm">Форма, содержащая файл, загружаемый пользователем.</param>
    /// <returns>Информация о загруженном файле, включая URL и MIME-тип.</returns>
    /// <remarks>
    ///     Запрос должен быть отправлен с <c>Content-Type: multipart/form-data</c>.
    ///     Пример запроса в Swagger:
    ///     <code>
    ///     POST /raids/1/upload?fileType=Screenshot
    ///     Form Data:
    ///         file: [выберите файл]
    ///     </code>
    /// </remarks>
    [HttpPost("[action]")]
    [Consumes("multipart/form-data")]
    public async Task<FileDto> UploadAsync([FromRoute] int raidId, [FromQuery] RaidFileType fileType, [FromForm] FileUploadForm fileForm)
    {
        var targetService = _serviceFactory.GetRaidFileService(fileType);

        await using var stream = fileForm.File.OpenReadStream();
        var request = new FileUploadRequest(stream, fileForm.File.FileName, fileForm.File.ContentType);
        return await targetService.UploadAsync(request, raidId);
    }
}
