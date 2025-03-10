﻿using System;
using System.IO;
using System.Threading.Tasks;

using Avalonia.Platform;

using SmartGenealogy.Core.Helper;
using SmartGenealogy.Core.Models.FileInterfaces;

namespace SmartGenealogy.Models;

public readonly record struct AvaloniaResource(
    Uri UriPath,
    UnixFileMode WriteUnixFileMode = UnixFileMode.None)
{
    /// <summary>
    /// File name component of the Uri path.
    /// </summary>
    public string FileName => Path.GetFileName(UriPath.ToString());

    /// <summary>
    /// File path relative to the 'Assets' folder.
    /// </summary>
    public Uri RelativeAssetPath =>
        new Uri("avares://SmartGenealogy/Assets/").MakeRelativeUri(UriPath);

    public AvaloniaResource(string uriPath, UnixFileMode writeUnixFileMode = UnixFileMode.None)
        : this(new Uri(uriPath), writeUnixFileMode)
    {
    }

    /// <summary>
    /// Opens a stream to this resource.
    /// </summary>
    public Stream Open() => AssetLoader.Open(UriPath);

    /// <summary>
    /// Extracts this resource to a target file path.
    /// </summary>
    public async Task ExtractTo(FilePath outputPath, bool overwrite = true)
    {
        if (outputPath.Exists)
        {
            // Skip if not overwriting
            if (!overwrite) return;
            // Otherwise delete the file
            outputPath.Delete();
        }
        var stream = AssetLoader.Open(UriPath);
        await using var fileStream = File.Create(outputPath);
        await stream.CopyToAsync(fileStream);
        // Write permissions
        if (!Compat.IsWindows && Compat.IsUnix && WriteUnixFileMode != UnixFileMode.None)
        {
            File.SetUnixFileMode(outputPath, WriteUnixFileMode);
        }
    }

    /// <summary>
    /// Extracts this resource to the output directory.
    /// </summary>
    public Task ExtractToDir(DirectoryPath outputDir, bool overwrite = true)
    {
        return ExtractTo(outputDir.JoinFile(FileName), overwrite);
    }
}