using JetBrains.Annotations;

using SmartGenealogy.Native.Abstractions;
using SmartGenealogy.Native.Windows.FileOperations;
using SmartGenealogy.Native.Windows.Interop;

namespace SmartGenealogy.Native.Windows;

[PublicAPI]
public class NativeRecycleBinProvider : INativeRecycleBinProvider
{
    /// <inheritdoc />
    public void MoveFileToRecycleBin(string path, NativeFileOperationFlags flags = 0)
    {
        using var fo = new FileOperationWrapper();

        var fileOperationFlags = default(uint);
        flags.ToWindowsFileOperationFlags(ref fileOperationFlags);

        fo.SetOperationFlags(
            (FileOperationFlags)fileOperationFlags | FileOperationFlags.FOFX_RECYCLEONDELETE);
        fo.DeleteItem(path);
        fo.PerformOperations();
    }

    /// <inheritdoc />
    public Task MoveFileToRecycleBinAsync(string path, NativeFileOperationFlags flags = 0)
    {
        return Task.Run(() => MoveFileToRecycleBin(path, flags));
    }

    /// <inheritdoc />
    public void MoveFilesToRecycleBin(IEnumerable<string> paths, NativeFileOperationFlags flags = 0)
    {
        using var fo = new FileOperationWrapper();

        var fileOperationFlags = default(uint);
        flags.ToWindowsFileOperationFlags(ref fileOperationFlags);

        fo.SetOperationFlags(
            (FileOperationFlags)fileOperationFlags | FileOperationFlags.FOFX_RECYCLEONDELETE);
        fo.DeleteItems(paths.ToArray());
        fo.PerformOperations();
    }

    /// <inheritdoc />
    public Task MoveFilesToRecycleBinAsync(
        IEnumerable<string> paths,
        NativeFileOperationFlags flags = 0)
    {
        return Task.Run(() => MoveFilesToRecycleBin(paths, flags));
    }

    /// <inheritdoc />
    public void MoveDirectoryToRecycleBin(string path, NativeFileOperationFlags flags = 0)
    {
        using var fo = new FileOperationWrapper();

        var fileOperationFlags = default(uint);
        flags.ToWindowsFileOperationFlags(ref fileOperationFlags);

        fo.SetOperationFlags(
            (FileOperationFlags)fileOperationFlags | FileOperationFlags.FOFX_RECYCLEONDELETE);
        fo.DeleteItem(path);
        fo.PerformOperations();
    }

    /// <inheritdoc />
    public Task MoveDirectoryToRecycleBinAsync(string path, NativeFileOperationFlags flags = 0)
    {
        return Task.Run(() => MoveDirectoryToRecycleBin(path, flags));
    }

    /// <inheritdoc />
    public void MoveDirectoriesToRecycleBin(IEnumerable<string> paths, NativeFileOperationFlags flags = 0)
    {
        using var fo = new FileOperationWrapper();

        var fileOperationFlags = default(uint);
        flags.ToWindowsFileOperationFlags(ref fileOperationFlags);

        fo.SetOperationFlags(
            (FileOperationFlags)fileOperationFlags | FileOperationFlags.FOFX_RECYCLEONDELETE);
        fo.DeleteItems(paths.ToArray());
        fo.PerformOperations();
    }

    /// <inheritdoc />
    public Task MoveDirectoriesToRecycleBinAsync(
        IEnumerable<string> paths,
        NativeFileOperationFlags flags = 0)
    {
        return Task.Run(() => MoveDirectoriesToRecycleBin(paths, flags));
    }
}