using Freaks.Dal.Common.Interfaces;
using Freaks.Users.Contracts;
using Freaks.Users.Persistence;

namespace Freaks.Users.Dal;

public interface IUserProvider : IBaseProvider<User, Guid, UserDbContext>
{
}