using Freaks.Portal.Bll.Interfaces.Loot;
using Freaks.Portal.Dal.Interfaces.Loot;
using Freaks.Portal.SharedContracts.Dto.Loot;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Loot;

/// <summary>
///     Сервис, реализующий бизнес-логику для работы с предметами добычи (лутом).
///     Использует провайдер для доступа к данным и AutoMapper для преобразования сущностей в DTO.
/// </summary>
public class LootItemService : ILootItemService
{
    private readonly IMapper _mapper;
    private readonly ILootItemProvider _provider;

    /// <summary>
    ///     Инициализирует новый экземпляр сервиса <see cref="LootItemService" />.
    /// </summary>
    /// <param name="mapper">Сервис отображения объектов (AutoMapper).</param>
    /// <param name="provider">Провайдер данных предметов добычи.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из аргументов равен <c>null</c>.</exception>
    public LootItemService(
        IMapper mapper,
        ILootItemProvider provider)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public async Task<IList<LootItemDto>> GetListAsync()
    {
        var result = await _provider.GetListAsync();

        return _mapper.Map<IList<LootItemDto>>(result);
    }
}