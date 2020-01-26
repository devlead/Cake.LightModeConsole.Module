// Install modules
#module nuget:?package=Cake.DotNetTool.Module&version=0.4.0

// Install .NET Core Global tools.
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=5.1.3"

DirectoryPath testDirectoryPath = Directory("./tests/integration");
DirectoryPath testToolDirectoryPath = testDirectoryPath.Combine("tools");
FilePath projectFilePath = File("./src/Cake.LightModeConsole.Module/Cake.LightModeConsole.Module.csproj");

FilePath testFilePath = testDirectoryPath.CombineWithFilePath("build.cake");

string target = Argument("target", "default");

string semVer = GitVersion()?.SemVer;

Task("Clean")
    .Does(() => {
    if (DirectoryExists(testToolDirectoryPath))
    {
        DeleteDirectory(testToolDirectoryPath,
            new DeleteDirectorySettings {
                Force = true,
                Recursive = true 
            });
    }
});

Task("Pack")
    .IsDependentOn("Clean")
    .Does(() => {
    DotNetCorePack(
        projectFilePath.FullPath,
        new DotNetCorePackSettings {
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                                .WithProperty("Version", semVer)   
        });
});

Task("Run-Integration-Tests")
    .IsDependentOn("Pack")
    .Does(() => {
    CakeExecuteScript(testFilePath,
        new CakeSettings {
            WorkingDirectory = testDirectoryPath,
            EnvironmentVariables = {
                { "CAKE_LIGHTMODECONSOLE_MODULE_VERSION", semVer }
            },
            Arguments = {
                { "--bootstrap", "true" }
            }
        });

    CakeExecuteScript(testFilePath,
        new CakeSettings {
            WorkingDirectory = testDirectoryPath,
            EnvironmentVariables = {
                { "CAKE_LIGHTMODECONSOLE_MODULE_VERSION", semVer }
            }
        });
});

Task("Default")
    .IsDependentOn("Run-Integration-Tests");


RunTarget(target);