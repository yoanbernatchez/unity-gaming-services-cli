namespace Unity.Services.Cli.GameServerHosting.UnitTest;

public static class GameServerHostingUnitTestsConstants
{
    public const string TestAccessToken = "testAccessToken";
    public const string ValidProjectId = "00000000-0000-0000-0000-000000000010";
    public const string InvalidProjectId = "00000000-0000-0000-0000-000000000011";
    public const string ValidEnvironmentName = "production";
    public const string ValidEnvironmentId = "00000000-0000-0000-0000-000000000000";
    public const string InvalidEnvironmentId = "00000000-0000-0000-0000-000000000001";
    public const string ValidBuildName = "Build One";
    public const string ValidBuildId = "11";
    public const string InvalidBuildId = "1234";
    public const string ValidBucketId = "11111000-0000-0000-0000-000000000000";
    public const string ValidReleaseId = "00000000-1100-0000-0000-000000000000";
    public const string ValidContainerTag = "v1";

    // Build File constants
    public const long BuildWithOneFileId = 121;
    public const long BuildWithTwoFilesId = 122;
    public const long ValidBuildIdBucket = 201;
    public const long ValidBuildIdContainer = 202;
    public const long ValidBuildIdFileUpload = 203;
    public const long SyncingBuildId = 333;

    // Filenames
    public const string BuildWithOneFileFileName = "game_binary.txt";
    public const string BuildWithOneToBeDeletedFileFileName = "file_to_be_deleted.txt";

    // Fleet specific constants
    public const string ValidFleetId = "00000000-0000-0000-1000-000000000000";
    public const string ValidFleetId2 = "00000000-0000-0000-1100-000000000000";

    public const string InvalidFleetId = "00000000-0000-0000-2222-000000000000";

    public const string ValidFleetName = "Fleet One";
    public const string ValidFleetName2 = "Fleet Two";

    public const string OsNameLinux = "Linux";

    // Build Configuration specific constants
    public const long ValidBuildConfigurationId = 1L;
    public const long ValidBuildConfigurationBuildId = 1L;
    public const long InvalidBuildConfigurationId = 666L;
    public const string ValidBuildConfigurationName = "Build Configuration One";
    public const string ValidBuildConfigurationBinaryPath = "/test.exe";
    public const string ValidBuildConfigurationCommandLine = "--init test";
    public const string ValidBuildConfigurationConfiguration = "hello:world";

    public const string ValidBuildConfigurationQueryType = "a2s";


    public const string ValidRegionId = "00000000-0000-0000-0000-000000000000";
    public const string ValidRegionId2 = "00000000-0000-0000-0000-000000000001";
    public const string InvalidRegionId = "00000000-0000-0000-0000-000000000002";
    public const string InvalidUuid = "00000000-0000-0000-ZZZZ-000000000000";

    public const string ValidTemplateRegionName = "us-west1";
    public const string ValidTemplateRegionId = "00000000-0000-0000-aaaa-110000000000";

    public const string ValidTemplateRegionName2 = "europe-west2";
    public const string ValidTemplateRegionId2 = "00000000-0000-0000-aaaa-220000000000";

    public const string ValidFleetRegionId = "00000000-0000-0000-aaaa-300000000000";

    public const long ValidServerId = 123456L;
    public const long InvalidServerId = 666L;

    public const long ValidMachineId = 654321L;
    public const long InvalidMachineId = 666L;

    public const long ValidLocationId = 111111L;
    public const string ValidLocationName = "us-west1";
}
