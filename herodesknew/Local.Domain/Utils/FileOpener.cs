using System.Diagnostics;

namespace herodesknew.Local.Domain.Utils;

public class FileOpener
{
    public static void Open(string fileName)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        });
    }
}
