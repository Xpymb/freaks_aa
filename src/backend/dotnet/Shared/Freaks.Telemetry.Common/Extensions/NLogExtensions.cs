using Freaks.Telemetry.Common.LayoutRenderers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Web;

namespace Freaks.Telemetry.Common.Extensions;

public static class NLogExtensions
{
    public static WebApplicationBuilder AddNLogCommon(this WebApplicationBuilder builder)
    {
        LayoutRenderer.Register<IndentedMessageLayoutRenderer>("indent-message");

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }
}