namespace Freaks.Files.SharedContracts.Requests;

/// <summary>
///     DTO-модель для передачи данных о загружаемом файле.
///     Используется для абстракции над <c>IFormFile</c> в бизнес-логике.
/// </summary>
/// <param name="Content">Поток содержимого файла.</param>
/// <param name="FileName">Имя файла, включая расширение (например, <c>image.png</c>).</param>
/// <param name="ContentType">MIME-тип содержимого (например, <c>image/png</c>).</param>
public record FileUploadRequest(
    Stream Content,
    string FileName,
    string ContentType);