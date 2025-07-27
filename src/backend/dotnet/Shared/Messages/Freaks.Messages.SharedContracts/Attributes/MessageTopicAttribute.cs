namespace Freaks.Messages.SharedContracts.Attributes;

/// <summary>
///     Атрибут для указания темы сообщения в системе обмена сообщениями.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MessageTopicAttribute : Attribute
{
    /// <summary>
    ///     Имя темы, связанной с помеченным классом.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="MessageTopicAttribute" />,
    ///     задавая имя темы сообщения.
    /// </summary>
    /// <param name="name">Имя темы сообщения.</param>
    public MessageTopicAttribute(string name)
    {
        Name = name;
    }
}