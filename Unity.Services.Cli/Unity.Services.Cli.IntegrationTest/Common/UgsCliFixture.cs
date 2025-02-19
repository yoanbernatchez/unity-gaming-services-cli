using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Unity.Services.Cli.Common.Networking;
using Unity.Services.Cli.IntegrationTest.Common;
using Unity.Services.Cli.MockServer;
using Unity.Services.Cli.MockServer.Common;

namespace Unity.Services.Cli.IntegrationTest;

/// <summary>
/// A test fixture to facilitate integration testing
/// </summary>
[TestFixture, Timeout(5 * 60 * 1000)]
public abstract class UgsCliFixture
{
#if DISABLE_CLI_REBUILD
    static Task? s_BuildCliTask = Task.CompletedTask;
#else
    /// <summary>
    /// Build task to rebuild CLI for mock server
    /// </summary>
    static Task? s_BuildCliTask;
#endif

    /// <summary>
    /// Returns the path to the configuration file used by the config module
    /// </summary>
    protected string ConfigurationFile => m_IntegrationConfig.ConfigurationFile;

    /// <summary>
    /// Returns the path to the configuration file used by the auth module
    /// </summary>
    protected string CredentialsFile => m_IntegrationConfig.CredentialsFile;

    protected readonly MockApi m_MockApi = new(NetworkTargetEndpoints.MockServer);

    readonly IntegrationConfig m_IntegrationConfig = new();

    [OneTimeTearDown]
    public void DisposeMockServer()
    {
        m_MockApi.Server?.Dispose();
    }
    [OneTimeTearDown]
    public void DisposeIntegrationConfig()
    {
        m_IntegrationConfig.Dispose();
    }

    [OneTimeSetUp]
    public async Task BuildCliIfNeeded()
    {
        if (s_BuildCliTask == null)
        {
            await TestContext.Progress.WriteLineAsync("Building UGS CLI...");
            s_BuildCliTask = UgsCliBuilder.Build();
            await s_BuildCliTask;
        }
    }

    protected void SetConfigValue(string key, string value)
    {
        m_IntegrationConfig.SetConfigValue(key, value);
    }

    protected void DeleteLocalConfig()
    {
        if (File.Exists(ConfigurationFile))
        {
            File.Delete(ConfigurationFile);
        }
    }

    protected void DeleteLocalCredentials()
    {
        if (File.Exists(CredentialsFile))
        {
            File.Delete(CredentialsFile);
        }
    }

    protected void SetupProjectAndEnvironment()
    {
        SetConfigValue("project-id", CommonKeys.ValidProjectId);
        SetConfigValue("environment-name", CommonKeys.ValidEnvironmentName);
    }

    protected static UgsCliTestCase GetLoggedInCli()
    {
        return new UgsCliTestCase()
            .Command($"login --service-key-id {CommonKeys.ValidServiceAccKeyId} --secret-key-stdin")
            .StandardInputWriteLine(CommonKeys.ValidServiceAccSecretKey);
    }

    protected UgsCliTestCase GetFullySetCli()
    {
        SetupProjectAndEnvironment();
        return GetLoggedInCli();
    }
}
