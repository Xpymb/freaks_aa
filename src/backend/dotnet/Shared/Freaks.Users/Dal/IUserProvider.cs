using Freaks.Dal.Common.Interfaces;
using Freaks.Users.Contracts;

namespace Freaks.Users.Dal;

public interface IUserProvider : IBaseProvider<User, Guid, UserDbContext>
{
}