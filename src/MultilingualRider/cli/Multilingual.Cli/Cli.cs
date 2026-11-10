using MultilingualExtension.Services;
using MultilingualExtension.Shared.Helpers;
using MultilingualExtension.Shared.Interfaces;
using MultilingualExtension.Shared.Models;
using MultilingualExtension.Shared.Services;
using MultilingualExtension.Shared.Services.FileConverters;

namespace Multilingual.Cli;

/// <summary>Entry point of the tool. Run is the seam the tests call in process.</summary>
public static class Cli
{
    const string SettingsFilename = "MultiLingualExtensionSettings.json";
    const string KeyVariable = "MULTILINGUAL_TRANSLATION_KEY";

    const string Usage = """
        Usage:
          multilingual sync <file|solution-folder>            [--solution-dir <dir>]
          multilingual translate <file|solution-folder>       [--solution-dir <dir>]
          multilingual sync-translate <file|solution-folder>  [--solution-dir <dir>]
          multilingual export <file> --status <NewOrNeedReview|New|NeedReview|Final|All> [--solution-dir <dir>]
          multilingual import <masterfile> --file <csv|xlsx>  [--solution-dir <dir>]
          multilingual native <file>                          [--solution-dir <dir>]
          multilingual --help
        """;

    sealed class Logger(TextWriter output) : IStatusPadLoger
    {
        public void WriteText(string logtext) => output.WriteLine(logtext);
    }

    sealed class UsageException(string message) : Exception(message);

    public static async Task<int> Run(string[] args, TextWriter output, TextWriter error)
    {
        try
        {
            if (args.Length == 1 && args[0] is "--help" or "-h")
            {
                output.WriteLine(Usage);
                return 0;
            }
            return await Execute(args, output, error);
        }
        catch (UsageException ex)
        {
            error.WriteLine(ex.Message);
            error.WriteLine(Usage);
            return 1;
        }
        catch (Exception ex)
        {
            error.WriteLine(ex.Message);
            return 1;
        }
    }

    static async Task<int> Execute(string[] args, TextWriter output, TextWriter error)
    {
        if (args.Length < 2)
            throw new UsageException(args.Length == 0 ? "Missing command." : $"Missing path for '{args[0]}'.");
        var command = args[0];
        if (command is not ("sync" or "translate" or "sync-translate" or "export" or "import" or "native"))
            throw new UsageException($"Unknown command '{command}'.");

        string status = null, importFile = null, solutionDir = null;
        for (var i = 2; i < args.Length; i += 2)
        {
            if (i + 1 >= args.Length)
                throw new UsageException($"Missing value for '{args[i]}'.");
            switch (args[i])
            {
                case "--status": status = args[i + 1]; break;
                case "--file": importFile = args[i + 1]; break;
                case "--solution-dir": solutionDir = args[i + 1]; break;
                default: throw new UsageException($"Unknown option '{args[i]}'.");
            }
        }
        if (command == "export" && status == null) throw new UsageException("export needs --status.");
        if (command == "import" && importFile == null) throw new UsageException("import needs --file.");

        var path = Path.GetFullPath(args[1]);
        if (!File.Exists(path) && !Directory.Exists(path))
        {
            error.WriteLine($"Not found: {path}");
            return 1;
        }
        var settings = LoadSettings(path, solutionDir == null ? null : Path.GetFullPath(solutionDir));
        var log = new Logger(output);

        switch (command)
        {
            case "export": return Report(await Export(path, MapStatus(status), settings, log, output), error);
            case "import": return Report(await Import(path, Path.GetFullPath(importFile), settings, log), error);
            case "native": return Report(Native(path, settings, log), error);
            default: return await RunOnMasters(command, path, settings, log, error);
        }
    }

    static int Report(Result<bool> result, TextWriter error)
    {
        if (result.WasSuccessful) return 0;
        error.WriteLine(result.ErrorMessage);
        return 1;
    }

    static string MapStatus(string status) => status switch
    {
        "NewOrNeedReview" => Globals.STATUS_COMMENT_NEW_OR_NEED_REVIEW,
        "New" => Globals.STATUS_COMMENT_NEW,
        "NeedReview" => Globals.STATUS_COMMENT_NEED_REVIEW,
        "Final" => Globals.STATUS_COMMENT_FINAL,
        "All" => Globals.STATUS_COMMENT_ALL_ROWS,
        _ => throw new UsageException($"Unknown status '{status}'."),
    };

    static SettingsService LoadSettings(string path, string solutionDir)
    {
        string dir;
        if (solutionDir != null)
        {
            if (!Directory.Exists(solutionDir))
                throw new Exception($"Solution directory not found: {solutionDir}");
            dir = solutionDir;
        }
        else
        {
            dir = Directory.Exists(path) ? path : Path.GetDirectoryName(path);
            while (dir != null && !File.Exists(Path.Combine(dir, SettingsFilename)))
                dir = Path.GetDirectoryName(dir);
            if (dir == null)
                throw new Exception($"No {SettingsFilename} found above {path}. Add --solution-dir <dir>.");
        }

        var settings = new SettingsService();
        settings.ReInit(dir);
        if (settings.ExtensionSettings == null)
            throw new Exception($"invalid settings file {Path.Combine(dir, SettingsFilename)}");
        var key = Environment.GetEnvironmentVariable(KeyVariable);
        if (!string.IsNullOrEmpty(key))
            settings.ExtensionSettings.TranslationServiceMsoftKey = key;
        return settings;
    }

    static async Task<int> RunOnMasters(string command, string path, ISettingsService settings, IStatusPadLoger log, TextWriter error)
    {
        var masters = Directory.Exists(path) ? FindMasters(path, settings.ExtensionSettings.MasterLanguageCode) : [path];
        if (masters.Count == 0)
        {
            error.WriteLine($"No master resource files found in {path}");
            return 1;
        }

        var failed = 0;
        foreach (var master in masters)
        {
            var ok = true;
            try
            {
                if (command is "sync" or "sync-translate")
                    ok = Check(await new SyncFileService().SyncFile(master, log, settings), master, error);
                if (ok && command is "translate" or "sync-translate")
                    ok = Check(await new TranslationService().TranslateFile(master, log, settings), master, error);
            }
            catch (Exception ex)
            {
                // A failure in one resource set must not stop the rest of the run.
                error.WriteLine($"{master}: {ex.Message}");
                ok = false;
            }
            if (!ok) failed++;
        }
        if (failed > 0)
            error.WriteLine($"{failed} of {masters.Count} resource sets failed");
        return failed > 0 ? 1 : 0;
    }

    static bool Check(Result<bool> result, string file, TextWriter error)
    {
        if (result.WasSuccessful) return true;
        error.WriteLine($"{file}: {result.ErrorMessage}");
        return false;
    }

    static List<string> FindMasters(string folder, string masterLanguageCode)
    {
        var found = new List<string>();
        void Walk(string dir)
        {
            foreach (var file in Directory.GetFiles(dir))
            {
                if (file.EndsWith(".resx", StringComparison.OrdinalIgnoreCase) && !RegExHelper.ValidateFilenameIsTargetType(file).Success)
                    found.Add(file);
                else if (file.EndsWith(".resw", StringComparison.OrdinalIgnoreCase) && Path.GetFileName(dir) == masterLanguageCode)
                    found.Add(file);
            }
            foreach (var sub in Directory.GetDirectories(dir))
            {
                var name = Path.GetFileName(sub);
                if (name is not ("bin" or "obj") && !name.StartsWith('.'))
                    Walk(sub);
            }
        }
        Walk(folder);
        found.Sort(StringComparer.Ordinal);
        return found;
    }

    static async Task<Result<bool>> Export(string path, string status, ISettingsService settings, IStatusPadLoger log, TextWriter output)
    {
        if (!File.Exists(path)) return new Result<bool>("export needs a resource file, not a folder.");
        if (settings.ExtensionSettings.ExportFileType == 1)
            output.WriteLine("CSV export is not supported, writing xlsx");
        return await new ExportService().ExportToFile(path, status, log, settings);
    }

    static async Task<Result<bool>> Import(string path, string importFile, ISettingsService settings, IStatusPadLoger log)
    {
        if (!File.Exists(importFile)) return new Result<bool>($"Import file not found: {importFile}");
        if (importFile.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            var target = importFile[..^5];
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (!File.Exists(target))
                return new Result<bool>($"Language file not found for the import file: {target}");
            if (!string.Equals(Path.GetDirectoryName(target), Path.GetDirectoryName(path), comparison))
                return new Result<bool>($"{Path.GetFileName(target)} is not in the same folder as {Path.GetFileName(path)}");
        }
        return await new ImportService().ImportToResx(importFile, log, settings);
    }

    static Result<bool> Native(string path, ISettingsService settings, IStatusPadLoger log)
    {
        if (!path.EndsWith(".resx", StringComparison.OrdinalIgnoreCase) || !File.Exists(path) || RegExHelper.ValidateFilenameIsTargetType(path).Success)
            return new Result<bool>("native needs a master .resx file.");
        var folder = Path.GetDirectoryName(path);
        var s = settings.ExtensionSettings;
        Generate(ResxConverters.Android, "Android", s.AndroidResourcesOutPutFolder, folder, log);
        Generate(ResxConverters.iOS, "Ios", s.IosResourcesOutPutFolder, folder, log);
        return new Result<bool>(true);
    }

    static void Generate(ResxConverter converter, string fallback, string configured, string folder, IStatusPadLoger log)
    {
        var outFolder = string.IsNullOrEmpty(configured) ? null : Path.GetFullPath(configured, folder);
        if (outFolder == null || !Directory.Exists(outFolder))
        {
            log.WriteText($"Output folder {configured} does not exist use standard");
            outFolder = Path.Combine(folder, fallback);
        }
        log.WriteText($"Start export all {fallback} translations to the folder {outFolder}");
        converter.Convert(folder, outFolder, log);
    }
}
