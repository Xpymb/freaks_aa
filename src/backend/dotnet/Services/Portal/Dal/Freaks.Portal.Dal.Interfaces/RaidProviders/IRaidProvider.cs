using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.RaidEntities;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.RaidProviders;

public interface IRaidProvider : IBaseProvider<Raid, int, PortalDbContext>
{
    
}