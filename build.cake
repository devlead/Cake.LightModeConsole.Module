// Install modules
#module nuget:?package=Cake.DotNetTool.Module&version=0.4.0

// Install .NET Core Global tools.
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=5.1.3"

string target = Argument("target", "default");

Setup<BuildData>(
    context => {
        DirectoryPath testDirectoryPath = Directory("./tests/integration");
        DirectoryPath testToolDirectoryPath = testDirectoryPath.Combine("tools");
        FilePath projectFilePath = File("./src/Cake.LightModeConsole.Module/Cake.LightModeConsole.Module.csproj");
        FilePath testFilePath = testDirectoryPath.CombineWithFilePath("build.cake");

        string semVersion = GitVersion()?.NuGetVersion ?? "0.0.1";

        return new BuildData(
            testDirectoryPath,
            testToolDirectoryPath,
            testFilePath,
            projectFilePath,
            semVersion
        );
    }
);

Task("Clean")
    .Does<BuildData>((context, data) => {
    if (DirectoryExists(data.TestToolDirectory))
    {
        DeleteDirectory(data.TestToolDirectory,
            new DeleteDirectorySettings {
                Force = true,
                Recursive = true
            });
    }
});

Task("Pack")
    .IsDependentOn("Clean")
    .Does<BuildData>((context, data) => {
    DotNetCorePack(
        data.ProjectFile.FullPath,
        new DotNetCorePackSettings {
            MSBuildSettings = data.MSBuildSettings
        });
});

Task("Run-Integration-Tests")
    .IsDependentOn("Pack")
    .Does<BuildData>((context, data) => {
    CakeExecuteScript(data.TestScriptPath,
        new CakeSettings {
            WorkingDirectory = data.TestDirectory,
            EnvironmentVariables = {
                { "CAKE_LIGHTMODECONSOLE_MODULE_VERSION", data.SemVersion }
            },
            Arguments = {
                { "--bootstrap", "true" }
            }
        });

    CakeExecuteScript(data.TestScriptPath,
        new CakeSettings {
            WorkingDirectory = data.TestDirectory,
            EnvironmentVariables = {
                { "CAKE_LIGHTMODECONSOLE_MODULE_VERSION", data.SemVersion }
            }
        });
});

Task("Default")
    .IsDependentOn("Run-Integration-Tests");


RunTarget(target);




public class BuildData
{
    public DirectoryPath TestDirectory { get; }
    public DirectoryPath TestToolDirectory { get; }
    public FilePath TestScriptPath { get; }
    public FilePath ProjectFile { get; }
    public DotNetCoreMSBuildSettings MSBuildSettings { get; }
    public string SemVersion { get; }

    public BuildData(
        DirectoryPath testDirectory,
        DirectoryPath testToolDirectory,
        FilePath testScriptPath,
        FilePath projectFile,
        string semVersion
    )
    {
        TestDirectory = testDirectory;
        TestToolDirectory = testToolDirectory;
        TestScriptPath = testScriptPath;
        ProjectFile = projectFile;
        SemVersion = semVersion;
        MSBuildSettings = new DotNetCoreMSBuildSettings()
                                .WithProperty("Version", semVersion);
    }
}