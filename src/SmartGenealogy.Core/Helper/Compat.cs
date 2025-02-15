using System.Runtime.Versioning;

using Semver;

namespace SmartGenealogy.Core.Helper;

/// <summary>
/// Compatibility layer for checks and file paths on different platforms.
/// </summary>
public static class Compat
{
    private const string AppName = "Smart Genealogy";

    public static SemVersion AppVersion { get; set; }

    // OS Platform
    public static PlatformKind Platform { get; }

    [SupportedOSPlatformGuard("windows")]
    public static bool IsWindows => Platform.HasFlag(PlatformKind.Windows);

    [SupportedOSPlatformGuard("linux")]
    public static bool IsLinux => Platform.HasFlag(PlatformKind.Linux);

    [SupportedOSPlatformGuard("macos")]
    public static bool IsMacOS => Platform.HasFlag(PlatformKind.MacOS);

    [UnsupportedOSPlatformGuard("windows")]
    public static bool IsUnix => Platform.HasFlag(PlatformKind.Unix);

    public static bool IsArm => Platform.HasFlag(PlatformKind.Arm);
    public static bool IsX64 => Platform.HasFlag(PlatformKind.X64);
}