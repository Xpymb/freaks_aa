namespace Freaks.Users.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class UserRoleNameAttribute : Attribute
{
    public string Name { get; }

    public UserRoleNameAttribute(string name)
    {
        Name = name;
    }
}