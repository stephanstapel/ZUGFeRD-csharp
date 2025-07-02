using System.Runtime.InteropServices;

namespace s2industries.ZUGFeRD.PDF.Test;

[TestClass]
public class FontInfoProviderTests
{
    public TestContext TestContext { get; set; }

    [TestMethod]
    public void CheckGetInstalledFonts()
    {
        List<string> fontDirectories = FontInfoProvider.GetDefaultFontDirectories()
            .Where(Directory.Exists)
            .ToList();

        // valid font directories exist
        Assert.IsTrue(fontDirectories.Count > 0);

        List<string> fontFiles = fontDirectories.SelectMany(directory =>
                Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                    .Where(file =>
                        file.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) ||
                        file.EndsWith(".otf", StringComparison.OrdinalIgnoreCase)))
            .ToList();

        // ttf/otf are installed
        Assert.IsTrue(fontFiles.Count > 0);

        List<InstalledFont> installedFonts = FontInfoProvider.GetInstalledFonts();

        // same for the method that reads the fonts
        Assert.IsTrue(installedFonts.Count > 0);

        // no exceptional behavior when reading the files that leads to skipped fonts
        Assert.AreEqual(installedFonts.Count, fontFiles.Count);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            // Arial, Verdana and Trebuchet MS are installed per default on win and osx
            Assert.IsTrue(installedFonts.Any(font => font.FamilyName == "Arial"));
            Assert.IsTrue(installedFonts.Any(font => font.FamilyName == "Verdana"));
            Assert.IsTrue(installedFonts.Any(font => font.FamilyName == "Trebuchet MS"));
        }
        else
        {
            // DejaVu Sans is the most common font on linux os
            Assert.IsTrue(installedFonts.Any(font => font.FamilyName == "DejaVu Sans"));
        }
    }
}
