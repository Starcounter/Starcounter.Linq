///
/// Private namespace
///
{
    ///
    /// Root path configuration
    ///
    DirectoryPath rootDirectory = Context.GetCallerInfo().SourceFilePath.GetDirectory();
    string rootPath = rootDirectory.FullPath;

    ///
    /// Argument parsing
    ///
    string configuration = Argument("configuration", "Debug");
    string nugetConfigFile = Argument("nugetConfigFile", "");

    string starNugetPath = Argument("starNugetPath", "");
    if (string.IsNullOrEmpty(starNugetPath))
    {
        string starNugetEnvVar = EnvironmentVariable("STAR_NUGET");
        starNugetPath = string.IsNullOrEmpty(starNugetEnvVar) ? $"{rootPath}/%STAR_NUGET%" : starNugetEnvVar;
    }

    ///
    /// Dependent targets
    ///
    Task("BuildLinq")
        .IsDependentOn("RestoreLinq")
        .IsDependentOn("BuildLinqI");

    Task("TestLinq")
        .IsDependentOn("RestoreLinq")
        .IsDependentOn("BuildLinqI")
        .IsDependentOn("TestLinqI");

    Task("PackLinq")
        .IsDependentOn("RestoreLinq")
        .IsDependentOn("BuildLinqI")
        .IsDependentOn("TestLinqI")
        .IsDependentOn("PackLinqI");

    ///
    /// Task for restoring Starcounter.Linq
    ///
    Task("RestoreLinq").Does(() =>
    {
        var settings = new DotNetCoreRestoreSettings
        {
            NoCache = true
        };

        if (!string.IsNullOrEmpty(nugetConfigFile))
        {
            settings.ConfigFile = nugetConfigFile;
        }

        DotNetCoreRestore($"{rootPath}/Starcounter.Linq.sln", settings);
    });

    ///
    /// Task for building Starcounter.Linq
    ///
    Task("BuildLinqI").Does(() =>
    {
        var settings = new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            NoRestore = true
        };

        DotNetCoreBuild($"{rootPath}/Starcounter.Linq.sln", settings);
    });

    ///
    /// Task for testing Starcounter.Linq
    ///
    Task("TestLinqI").DoesForEach(GetFiles(rootPath + "/**/*Tests.csproj"), (csprojFile) =>
    {
        var testDirectory = csprojFile.GetDirectory();
        var projNameWithoutExtension = csprojFile.GetFilenameWithoutExtension();

        var settings = new DotNetCoreTestSettings
        {
            Configuration = configuration,
            DiagnosticFile = $"{testDirectory}/{projNameWithoutExtension}.log",
            NoBuild = true,
            NoRestore = true,
            ArgumentCustomization = args => args.Append("--verbosity normal")
        };

         DotNetCoreTest(csprojFile.FullPath, settings);
    });

    ///
    /// Task for packaging Starcounter.Linq
    ///
    Task("PackLinqI").Does(() =>
    {
        var projectFile = $"{rootPath}/src/Starcounter.Linq/Starcounter.Linq.csproj";

        var settings = new DotNetCorePackSettings
        {
            Configuration = configuration,
            IncludeSymbols = true,
            OutputDirectory = starNugetPath,
            NoRestore = true,
            NoBuild = true
        };

        DotNetCorePack(projectFile, settings);
    });

    ///
    /// Run targets if invoked as self-containment script
    ///
    if (!Tasks.Any(t => t.Name.Equals("Bifrost")))
    {
        // Read targets argument
        IEnumerable<string> targetsArg = Argument("targets", "Pack").Split(new Char[]{',', ' '}).Where(s => !string.IsNullOrEmpty(s));

        // Self-containment dependent targets
        Task("Restore").IsDependentOn("RestoreLinq");
        Task("Build").IsDependentOn("BuildLinq");
        Task("Test").IsDependentOn("TestLinq");
        Task("Pack").IsDependentOn("PackLinq");

        // Run target
        foreach (string t in targetsArg)
        {
            RunTarget(t);
        }
    }
}