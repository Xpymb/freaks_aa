using System.Reflection;
using Freaks.Messages.SharedContracts.Attributes;
using Freaks.Messages.SharedContracts.Messages;

namespace Freaks.Messages.Bll.Helpers;

/// <summary>
///     Помощник для работы с темами сообщений.
/// </summary>
public static class TopicHelper
{
    /// <summary>
    ///     Возвращает имя темы для сообщения на основе атрибута <see cref="MessageTopicAttribute" />.
    /// </summary>
    /// <typeparam name="T">Тип сообщения, наследуемый от <see cref="BaseMessage" />.</typeparam>
    /// <param name="message">Экземпляр сообщения.</param>
    /// <returns>Значение свойства <see cref="MessageTopicAttribute.Name" />.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Бросается, если на типе сообщения не найден атрибут <see cref="MessageTopicAttribute" />.
    /// </exception>
    public static string GetTopicName<T>(T message) where T : BaseMessage
    {
        var type = message.GetType();

        var attr = type.GetCustomAttribute<MessageTopicAttribute>();
        if (attr is null)
        {
            throw new InvalidOperationException($"Тип {type.FullName} не помечен атрибутом {nameof(MessageTopicAttribute)}");
        }

        return attr.Name;
    }
}