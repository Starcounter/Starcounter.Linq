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
    string msbuildPropertyNoWarning = Argument("msbuildPropertyNoWarning", ""); // Semicolon (;) separated for multiple warnings
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
        var msBuildSettings = new DotNetCoreMSBuildSettings();

        if (!string.IsNullOrEmpty(msbuildPropertyNoWarning))
        {
            msBuildSettings.WithProperty("NoWarn", msbuildPropertyNoWarning);
        }

        var settings = new DotNetCoreRestoreSettings
        {
            MSBuildSettings = msBuildSettings,
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
            ArgumentCustomization = args => args.Append("--verbosity normal"),
            EnvironmentVariables = GetConfiguration()
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
    /// Utilities
    ///
    Dictionary<string, string> GetConfiguration()
    {
        Dictionary<string, string> envVars = new Dictionary<string, string>();

        if (EnvironmentVariable("StarcounterBin") == null)
        {
            string level0Binaries = null;

            if (IsRunningOnUnix())
            {
                level0Binaries = rootPath + "/../level0/make/x64/" + configuration;
                envVars.Add("StarcounterBin", level0Binaries);
                envVars.Add("LD_LIBRARY_PATH", level0Binaries);
            }
            else if (IsRunningOnWindows())
            {
                level0Binaries = rootPath + "/../level0/msbuild/x64/" + configuration;
                envVars.Add("StarcounterBin", level0Binaries);
            }
            else
            {
                throw new Exception("Only Unix and Windows platforms are supported.");
            }

            envVars.Add("PATH", level0Binaries + System.IO.Path.PathSeparator + Environment.GetEnvironmentVariable("PATH"));

            // Check if Level0 binaries has been built.
            var di = new DirectoryInfo(level0Binaries);
            if (!di.Exists)
            {
                throw new Exception(string.Format("Exiting Nova execution step since Level0 has not been built. Content for environment variable StarcounterBin={0} does not exist.", di.FullName));
            }
        }

        return envVars;
    }

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