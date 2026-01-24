using Freaks.Messages.SharedContracts.Messages;

namespace Freaks.Messages.Bll.Interfaces;

/// <summary>
///     Сервис для публикации сообщений в систему обмена сообщениями.
/// </summary>
public interface IMessageService
{
    /// <summary>
    ///     Публикует сообщение заданного типа в соответствующую тему обмена.
    /// </summary>
    /// <typeparam name="T">Тип сообщения, наследуемый от <see cref="BaseMessage" />.</typeparam>
    /// <param name="message">Экземпляр сообщения для публикации.</param>
    Task PublishAsync<T>(T message) where T : BaseMessage;
}