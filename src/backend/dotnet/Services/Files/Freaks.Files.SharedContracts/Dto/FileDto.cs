namespace Freaks.Files.SharedContracts.Dto;

/// <summary>
///     DTO-модель, представляющая информацию о файле, размещённом в объектном хранилище.
///     Содержит как абсолютную ссылку, так и внутренний путь, а также метаинформацию о файле.
/// </summary>
/// <param name="FileUri">Внутренний путь к файлу в хранилище (например, <c>raids/1/screenshots/image.png</c>).</param>
/// <param name="FileName">Имя файла, включая расширение.</param>
/// <param name="ContentType">MIME-тип содержимого (например, <c>image/png</c>).</param>
public record FileDto(
    string FileUri,
    string FileName,
    string ContentType);