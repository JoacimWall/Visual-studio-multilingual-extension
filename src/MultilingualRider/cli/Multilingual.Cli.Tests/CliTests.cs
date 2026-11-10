using System.Xml.Linq;
using Xunit;

namespace Multilingual.Cli.Tests;

public class CliTests : IDisposable
{
    readonly string tmp = Path.Combine(Path.GetTempPath(), "ml-cli-" + Guid.NewGuid().ToString("N"));

    public CliTests() => Directory.CreateDirectory(tmp);

    public void Dispose() => Directory.Delete(tmp, true);

    // Copies the MAUI sample resource set into the temp folder; nothing under src/Samples is touched.
    string CopySample()
    {
        var dir = AppContext.BaseDirectory;
        while (!Directory.Exists(Path.Combine(dir, "src", "Samples")))
            dir = Path.GetDirectoryName(dir);
        var source = Path.Combine(dir, "src", "Samples", "MultilingualExtensionSammpleMAUI", "Resources", "Translation");
        foreach (var file in Directory.GetFiles(source, "*.resx"))
            File.Copy(file, Path.Combine(tmp, Path.GetFileName(file)));
        return tmp;
    }

    static (int Code, string Out, string Err) Run(params string[] args)
    {
        var o = new StringWriter();
        var e = new StringWriter();
        return (Cli.Run(args, o, e).GetAwaiter().GetResult(), o.ToString(), e.ToString());
    }

    static Dictionary<string, (string Value, string Status)> Rows(string file) =>
        XDocument.Load(file).Descendants("data").ToDictionary(
            d => (string)d.Attribute("name"),
            d => ((string)d.Element("value"), (string)d.Element("comment")));

    [Fact]
    public void Sync_gives_every_language_file_the_master_keys()
    {
        var dir = CopySample();
        var master = Path.Combine(dir, "Resource.resx");
        var fr = Path.Combine(dir, "Resource.fr.resx");

        var doc = XDocument.Load(fr);
        var removed = doc.Descendants("data").First();
        var removedName = (string)removed.Attribute("name");
        removed.Remove();
        doc.Root.Add(new XElement("data", new XAttribute("name", "ExtraKey"), new XElement("value", "x")));
        doc.Save(fr);

        var result = Run("sync", master, "--solution-dir", dir);

        Assert.Equal(0, result.Code);
        var expected = Rows(master).Where(r => r.Value.Status != "No translation").Select(r => r.Key).OrderBy(k => k).ToList();
        foreach (var file in new[] { fr, Path.Combine(dir, "Resource.sv-SE.resx") })
        {
            var rows = Rows(file);
            Assert.Equal(expected, rows.Keys.OrderBy(k => k).ToList());
        }
        Assert.Equal("New", Rows(fr)[removedName].Status);
        Assert.DoesNotContain("ExtraKey", Rows(fr).Keys);
    }

    [Fact]
    public void Sync_on_a_folder_syncs_the_master_sets_in_it()
    {
        var dir = CopySample();
        var fr = Path.Combine(dir, "Resource.fr.resx");
        var doc = XDocument.Load(fr);
        doc.Root.Add(new XElement("data", new XAttribute("name", "ExtraKey"), new XElement("value", "x")));
        doc.Save(fr);

        var result = Run("sync", dir, "--solution-dir", dir);

        Assert.Equal(0, result.Code);
        Assert.DoesNotContain("ExtraKey", Rows(fr).Keys);
    }

    [Fact]
    public void Export_then_import_gives_back_the_same_file()
    {
        var dir = CopySample();
        var master = Path.Combine(dir, "Resource.resx");
        var fr = Path.Combine(dir, "Resource.fr.resx");
        Assert.Equal(0, Run("sync", master, "--solution-dir", dir).Code);
        var before = Rows(fr);

        Assert.Equal(0, Run("export", fr, "--status", "All").Code);
        Assert.True(File.Exists(fr + ".xlsx"));
        var import = Run("import", master, "--file", fr + ".xlsx");

        Assert.Equal(0, import.Code);
        Assert.Equal(before, Rows(fr));
    }

    [Fact]
    public void Unknown_command_fails_on_stderr()
    {
        var result = Run("bogus", "x");

        Assert.Equal(1, result.Code);
        Assert.NotEmpty(result.Err);
    }
}
