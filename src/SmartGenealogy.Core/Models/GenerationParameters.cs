﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using SmartGenealogy.Core.Models.Api.Comfy;

namespace SmartGenealogy.Core.Models;

[JsonSerializable(typeof(GenerationParameters))]
public partial record GenerationParameters
{
    public string? PositivePrompt { get; set; }
    public string? NegativePrompt { get; set; }
    public int Steps { get; set; }
    public string? Sampler { get; set; }
    public double CfgScale { get; set; }
    public ulong Seed { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public string? ModelHash { get; set; }
    public string? ModelName { get; set; }
    public int FrameCount { get; set; }
    public int MotionBucketId { get; set; }
    public int VideoQuality { get; set; }
    public bool Lossless { get; set; }
    public int Fps { get; set; }
    public double OutputFps { get; set; }
    public double MinCfg { get; set; }
    public double AugmentationLevel { get; set; }
    public string? VideoOutputMethod { get; set; }

    public static bool TryParse(
        string? text,
        [NotNullWhen(true)] out GenerationParameters? generationParameters)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            generationParameters = null;
            return false;
        }

        try
        {
            generationParameters = Parse(text);
        }
        catch (Exception)
        {
            generationParameters = null;
            return false;
        }

        return true;
    }

    public static GenerationParameters Parse(string text)
    {
        var lines = text.Split('\n');

        if (lines.LastOrDefault() is not { } lastLine)
        {
            throw new ValidationException("fields line not found");
        }

        if (lastLine.StartsWith("Steps:") != true)
        {
            lines = text.Split("\r\n");
            lastLine = lines.LastOrDefault() ?? string.Empty;

            if (lastLine.StartsWith("Steps:") != true)
            {
                throw new ValidationException("Unable to locate starting marker of last line");
            }
        }

        // Join lines before last line, split at 'Negative prompt: '
        var joinedLines = string.Join("\n", lines[..^1]).Trim();

        // Apparently there is no space after the colon if value is empty, so check and add space here
        if (joinedLines.EndsWith("Negative prompt:"))
        {
            joinedLines += ' ';
        }

        var splitFirstPart = joinedLines.Split("Negative prompt: ", 2);

        var positivePrompt = splitFirstPart.ElementAtOrDefault(0)?.Trim();
        var negativePrompt = splitFirstPart.ElementAtOrDefault(1)?.Trim();

        // Parse last line
        var lineFields = ParseLine(lastLine);

        var generationParameters = new GenerationParameters
        {
            PositivePrompt = positivePrompt,
            NegativePrompt = negativePrompt,
            Steps = int.Parse(lineFields.GetValueOrDefault("Steps", "0")),
            Sampler = lineFields.GetValueOrDefault("Sampler"),
            CfgScale = double.Parse(lineFields.GetValueOrDefault("CFG scale", "0")),
            Seed = ulong.Parse(lineFields.GetValueOrDefault("Seed", "0")),
            ModelHash = lineFields.GetValueOrDefault("Model hash"),
            ModelName = lineFields.GetValueOrDefault("Model"),
        };

        if (lineFields.GetValueOrDefault("Size") is { } size)
        {
            var split = size.Split('x', 2);
            if (split.Length == 2)
            {
                generationParameters = generationParameters with
                {
                    Width = int.Parse(split[0]),
                    Height = int.Parse(split[1])
                };
            }
        }

        return generationParameters;
    }

    /// <summary>
    /// Parse A1111 metadata fields in a single line where
    /// fields are separated by commas and key-value pairs are separated by colons.
    /// i.e. "key1: value1, key2: value2"
    /// </summary>
    internal static Dictionary<string, string> ParseLine(string fields)
    {
        var dict = new Dictionary<string, string>();

        // Values may contain commas or colons
        foreach (var match in ParametersFieldsRegex().Matches(fields).Cast<Match>())
        {
            if (!match.Success)
                continue;

            var key = match.Groups[1].Value.Trim();
            var value = UnquoteValue(match.Groups[2].Value.Trim());

            dict.Add(key, value);
        }

        return dict;
    }

    /// <summary>
    /// Unquotes a quoted value field if required
    /// </summary>
    private static string UnquoteValue(string quotedField)
    {
        if (!(quotedField.StartsWith('"') && quotedField.EndsWith('"')))
        {
            return quotedField;
        }

        return JsonNode.Parse(quotedField)?.GetValue<string>() ?? "";
    }

    /// <summary>
    /// Converts current <see cref="Sampler"/> string to <see cref="ComfySampler"/> and <see cref="ComfyScheduler"/>.
    /// </summary>
    /// <returns></returns>
    public (ComfySampler sampler, ComfyScheduler scheduler)? GetComfySamplers()
    {
        if (Sampler is not { } source)
            return null;

        var scheduler = source switch
        {
            _ when source.Contains("Karras") => ComfyScheduler.Karras,
            _ when source.Contains("Exponential") => ComfyScheduler.Exponential,
            _ => ComfyScheduler.Normal,
        };

        var sampler = source switch
        {
            "LMS" => ComfySampler.LMS,
            "DDIM" => ComfySampler.DDIM,
            "UniPC" => ComfySampler.UniPC,
            "DPM fast" => ComfySampler.DpmFast,
            "DPM adaptive" => ComfySampler.DpmAdaptive,
            "Heun" => ComfySampler.Heun,
            _ when source.StartsWith("DPM2 a") => ComfySampler.Dpm2Ancestral,
            _ when source.StartsWith("DPM2") => ComfySampler.Dpm2,
            _ when source.StartsWith("DPM++ 2M SDE") => ComfySampler.Dpmpp2MSde,
            _ when source.StartsWith("DPM++ 2M") => ComfySampler.Dpmpp2M,
            _ when source.StartsWith("DPM++ 3M SDE") => ComfySampler.Dpmpp3MSde,
            _ when source.StartsWith("DPM++ 3M") => ComfySampler.Dpmpp3M,
            _ when source.StartsWith("DPM++ SDE") => ComfySampler.DpmppSde,
            _ when source.StartsWith("DPM++ 2S a") => ComfySampler.Dpmpp2SAncestral,
            _ => default
        };

        return (sampler, scheduler);
    }

    /// <summary>
    /// Return a sample parameters for UI preview
    /// </summary>
    public static GenerationParameters GetSample()
    {
        return new GenerationParameters
        {
            PositivePrompt = "(cat:1.2), by artist, detailed, [shaded]",
            NegativePrompt = "blurry, jpg artifacts",
            Steps = 30,
            CfgScale = 7,
            Width = 640,
            Height = 896,
            Seed = 124825529,
            ModelName = "ExampleMix7",
            ModelHash = "b899d188a1ac7356bfb9399b2277d5b21712aa360f8f9514fba6fcce021baff7",
            Sampler = "DPM++ 2M Karras"
        };
    }

    [GeneratedRegex("""\s*([\w ]+):\s*("(?:\\.|[^\\"])+"|[^,]*)(?:,|$)""")]
    private static partial Regex ParametersFieldsRegex();
}