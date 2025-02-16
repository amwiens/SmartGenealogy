using System.Runtime.InteropServices;

using NLog;

namespace SmartGenealogy.Core.Helper;

public static class SystemInfo
{
    public const long Gibibyte = 1024 * 1024 * 1024;
    public const long Mebibyte = 1024 * 1024;

    [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
    public static extern bool ShouldUseDarkMode();

    public static long? GetDiskFreeSpaceBytes(string path)
    {
        try
        {
            var drive = new DriveInfo(path);
            return drive.AvailableFreeSpace;
        }
        catch (Exception ex)
        {
            LogManager.GetCurrentClassLogger().Error(ex);
        }

        return null;
    }
}