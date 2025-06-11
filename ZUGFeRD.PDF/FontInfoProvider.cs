using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FontInfo;

namespace s2industries.ZUGFeRD.PDF
{
    internal static class FontInfoProvider
    {
        internal static IEnumerable<InstalledFont> GetInstalledFonts()
        {
            var result = new List<InstalledFont>();
            var fontDirs = GetFontDirectories();

            foreach (var dir in fontDirs)
            {
                if (!Directory.Exists(dir)) continue;

                var fontFiles = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
                    .Where(f => f.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase)
                             || f.EndsWith(".otf", StringComparison.OrdinalIgnoreCase));

                foreach (var file in fontFiles)
                {
                    try
                    {
                        Task<Font> fontTask = Font.CreateAsync(file);
                        fontTask.Wait();

                        result.Add(new InstalledFont
                        {
                            FamilyName = fontTask.Result.Details.FullName,
                            FilePath = file
                        });
                    }
                    catch
                    {
                        // Ignore unreadable or unsupported font fontFiles
                    }
                }
            }

            return result;
        } // !GetInstalledFonts()


        internal static IEnumerable<string> GetFontDirectories()
        {           
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                yield return Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                yield return "/usr/share/fonts";
                yield return "/usr/local/share/fonts";
                yield return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fonts");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                yield return "/System/Library/Fonts";
                yield return "/Library/Fonts";
                yield return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library/Fonts");
            }
        }  // !GetFontDirectories()
    }
}
