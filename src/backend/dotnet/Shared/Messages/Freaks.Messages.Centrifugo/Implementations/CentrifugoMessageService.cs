using Centrifugo.AspNetCore.Abstractions;
using Centrifugo.AspNetCore.Models.Request;
using Freaks.Messages.Bll.Helpers;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages;

namespace Freaks.Messages.Centrifugo.Implementations;

/// <summary>
///     Сервис для публикации сообщений через Centrifugo, реализующий <see cref="IMessageService" />.
/// </summary>
public class CentrifugoMessageService : IMessageService
{
    private readonly ICentrifugoClient _client;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="CentrifugoMessageService" />,
    ///     устанавливая клиент для взаимодействия с Centrifugo.
    /// </summary>
    /// <param name="client">Клиент <see cref="ICentrifugoClient" /> для публикации сообщений.</param>
    public CentrifugoMessageService(ICentrifugoClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <inheritdoc />
    public Task PublishAsync<T>(T message) where T : BaseMessage
    {
        var topicName = TopicHelper.GetTopicName(message);

        return _client.Publish(
            new PublishParams
            {
                Channel = topicName, Data = message,
            });
    }
}