using Microsoft.Extensions.Logging;
using Unity.Services.Cli.Common.Console;
using Unity.Services.Cli.Common.Input;
using Unity.Services.Cli.Common.Logging;
using Unity.Services.Cli.Common.Utils;
using Unity.Services.Cli.GameServerHosting.Model;
using Unity.Services.Cli.GameServerHosting.Service;

namespace Unity.Services.Cli.GameServerHosting.Handlers;

static class RegionTemplatesHandler
{
    public static async Task RegionTemplatesAsync(
        CommonInput input,
        IUnityEnvironment unityEnvironment,
        IGameServerHostingService service,
        ILogger logger,
        ILoadingIndicator loadingIndicator,
        CancellationToken cancellationToken
    )
    {
        await loadingIndicator.StartLoadingAsync("Fetching region list...",
            _ => RegionTemplatesAsync(input, unityEnvironment, service, logger, cancellationToken));
    }

    internal static async Task RegionTemplatesAsync(
        CommonInput input,
        IUnityEnvironment unityEnvironment,
        IGameServerHostingService service,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        // FetchIdentifierAsync handles null checks for project-id and environment
        var environmentId = await unityEnvironment.FetchIdentifierAsync(cancellationToken);

        await service.AuthorizeGameServerHostingService(cancellationToken);

        var regions = await service.FleetsApi.ListTemplateFleetRegionsAsync(
            Guid.Parse(input.CloudProjectId!),
            Guid.Parse(environmentId),
            cancellationToken: cancellationToken);

        logger.LogResultValue(new RegionTemplateListOutput(regions));
    }
}
