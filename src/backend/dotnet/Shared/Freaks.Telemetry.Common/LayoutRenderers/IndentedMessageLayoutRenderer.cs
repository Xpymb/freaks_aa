using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace Freaks.Telemetry.Common.LayoutRenderers;

/// <summary>
///     Выводит сообщение лога с автоматическим отступом для продолжающих строк,
///     выравнивая их по колонке сообщения (timestamp + level + logger = 58 символов).
/// </summary>
[LayoutRenderer("indent-message")]
public sealed class IndentedMessageLayoutRenderer : LayoutRenderer
{
    // timestamp(12) + |(1) + level(5) + |(1) + logger(35) + |(1) = 55
    private const string Continuation = "\n                                                       ";

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        var message = logEvent.FormattedMessage;

        if (!string.IsNullOrEmpty(message) && message.Contains('\n'))
            builder.Append(message.Replace("\r\n", "\n").Replace("\n", Continuation));
        else
            builder.Append(message);
    }
}
