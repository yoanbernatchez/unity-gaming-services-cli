using Spectre.Console;
using Unity.Services.Cli.Common.Utils;
using Unity.Services.Cli.Authoring.Input;
using Unity.Services.Cli.Authoring.Model;
using Unity.Services.Cli.Authoring.Service;
using Unity.Services.Cli.RemoteConfig.Model;
using Unity.Services.DeploymentApi.Editor;
using Unity.Services.RemoteConfig.Editor.Authoring.Core.Fetch;
using Unity.Services.RemoteConfig.Editor.Authoring.Core.Model;

namespace Unity.Services.Cli.RemoteConfig.Deploy;

class RemoteConfigFetchService : IFetchService
{
    readonly IUnityEnvironment m_UnityEnvironment;
    readonly IRemoteConfigFetchHandler m_FetchHandler;
    readonly ICliRemoteConfigClient m_RemoteConfigClient;
    readonly IDeployFileService m_DeployFileService;
    readonly IRemoteConfigScriptsLoader m_RemoteConfigScriptsLoader;
    readonly string m_DeployFileExtension;
    public string ServiceType { get; }
    public string ServiceName { get; }

    string IFetchService.FileExtension => m_DeployFileExtension;

    public RemoteConfigFetchService(
        IUnityEnvironment unityEnvironment,
        IRemoteConfigFetchHandler fetchHandler,
        ICliRemoteConfigClient remoteConfigClient,
        IDeployFileService deployFileService,
        IRemoteConfigScriptsLoader remoteConfigScriptsLoader
    )
    {
        m_UnityEnvironment = unityEnvironment;
        m_FetchHandler = fetchHandler;
        m_RemoteConfigClient = remoteConfigClient;
        m_DeployFileService = deployFileService;
        m_RemoteConfigScriptsLoader = remoteConfigScriptsLoader;
        ServiceType = "Remote Config";
        ServiceName = "remote-config";
        m_DeployFileExtension = ".rc";
    }

    public async Task<FetchResult> FetchAsync(
        FetchInput input,
        StatusContext? loadingContext,
        CancellationToken cancellationToken)
    {
        var environmentId = await m_UnityEnvironment.FetchIdentifierAsync(cancellationToken);
        m_RemoteConfigClient.Initialize(input.CloudProjectId!, environmentId, cancellationToken);
        var remoteConfigFiles = m_DeployFileService.ListFilesToDeploy(new[] { input.Path }, m_DeployFileExtension).ToList();

        var loadResult = await m_RemoteConfigScriptsLoader
            .LoadScriptsAsync(remoteConfigFiles, cancellationToken);
        var configFiles = loadResult.Loaded.ToList();

        loadingContext?.Status($"Fetching {ServiceType} Files...");

        var fetchResult = await m_FetchHandler.FetchAsync(
                input.Path,
                configFiles,
                input.DryRun,
                input.Reconcile,
                cancellationToken);

        var failed = fetchResult
            .Failed
            .Select(d => (RemoteConfigFile)d)
            .UnionBy(loadResult.Failed, f => f.Path)
            .ToList();

        return new FetchResult(
            fetchResult.Updated.Select(rce => GetDeployContent(rce, "Updated")).ToList(),
            fetchResult.Deleted.Select(rce => GetDeployContent(rce, "Deleted")).ToList(),
            fetchResult.Created.Select(rce => GetDeployContent(rce, "Updated")).ToList(),
            fetchResult.Fetched.Select(ToFetchedFile).ToList(),
            failed,
            input.DryRun);
    }

    static RemoteConfigFile ToFetchedFile(IRemoteConfigFile file)
    {
        var f = (RemoteConfigFile)file;
        f.Status = new DeploymentStatus(Statuses.Fetched, string.Empty, SeverityLevel.Success);
        f.Progress = 100f;
        return f;
    }

    static DeployContent GetDeployContent(RemoteConfigEntry entry, string status)
    {
        return new CliRemoteConfigEntry(entry.Key, "RemoteConfig Key", NormalizePath(entry.File), 100, status, string.Empty);
    }

    static string NormalizePath(IRemoteConfigFile? file)
    {
        return file is null ? "" : file.Path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }
}
