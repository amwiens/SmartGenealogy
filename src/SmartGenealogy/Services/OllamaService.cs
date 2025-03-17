using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using SmartGenealogy.Enums;
using SmartGenealogy.Models;

namespace SmartGenealogy.Services;

public class OllamaService
{
    private Process? _ollamaProcess;
    private static readonly HttpClient HttpClient = new();
    private string OllamaPath { get; set; }

    public delegate void ServiceStatusChangedHandler(ServiceStatus status, string? message);

    public event ServiceStatusChangedHandler? ServiceStatusChanged;

    public OllamaService()
    {
        OllamaPath = "";
    }

    private async Task Start()
    {
        var platform = DeviceInfo.Platform;

        if (platform == DevicePlatform.WinUI)
        {
            OllamaPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Ollama\ollama");
        }
        else if (platform == DevicePlatform.macOS || platform == DevicePlatform.MacCatalyst)
        {
            OllamaPath = @"/usr/local/bin/ollama";
        }

        var ollamaProcessCount = OllamaProcessCount();
        switch (ollamaProcessCount)
        {
            case 1:
                OnServiceStatusChanged(
                    ServiceStatus.Running, "Ollama is already running");
                return;

            case >= 2:
                OnServiceStatusChanged(
                    ServiceStatus.Failed, "Multiple instances of Ollama are running. Please close all instances and restart the app.");
                return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = OllamaPath,
            Arguments = "serve",
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            _ollamaProcess = Process.Start(startInfo);
        }
        catch (Win32Exception)
        {
            OnServiceStatusChanged(
                ServiceStatus.Failed, "Ollama is not installed");
        }
        catch (Exception ex)
        {
            OnServiceStatusChanged(
                ServiceStatus.Failed, $"An error occurred while starting ollama: {ex.Message}");
            return;
        }

        if (_ollamaProcess == null)
        {
            OnServiceStatusChanged(
                ServiceStatus.NotInstalled, "Ollama is not installed");
            return;
        }

        var isServerRunning = await IsOllamaServerRunning();
        if (isServerRunning)
        {
            OnServiceStatusChanged(
                ServiceStatus.Running, "Ollama has been successfully started");
        }
        else
        {
            const uint maxServerCheck = 10;
            uint serverCheck = 0;
            var serverAvailable = false;
            while (!serverAvailable && serverCheck != maxServerCheck)
            {
                serverAvailable = await IsOllamaServerRunning();
                serverCheck++;
                if (!serverAvailable)
                {
                    await Task.Delay(500);
                    continue;
                }

                OnServiceStatusChanged(
                    ServiceStatus.Running, "Ollama has been successfully started");
                return;
            }

            if (!serverAvailable)
            {
                OnServiceStatusChanged(
                    ServiceStatus.Failed, "Failed to establish connection with Ollama's local server");
            }
        }
    }

    private static uint OllamaProcessCount()
    {
        var ollamaProcesses = Process.GetProcessesByName("ollama");
        return (uint)ollamaProcesses.Length;
    }

    private static async Task<bool> IsOllamaServerRunning()
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:11434/api/version");
            return response.StatusCode == HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }

    public void Stop()
    {
        var ollamaProcessList = Process.GetProcessesByName("ollama");
        foreach (var process in ollamaProcessList)
        {
            KillProcess(process);
        }
    }

    private static void KillProcess(Process? process)
    {
        if (process == null || process.HasExited) return;
        process.Kill();
        process.Dispose();
    }

    public async Task StartWithDelay(TimeSpan delay)
    {
        await Task.Delay(delay);
        await Start();
    }

    private void OnServiceStatusChanged(ServiceStatus status, string? message = null)
    {
        ServiceStatusChanged?.Invoke(status, message);
    }

    public async Task<bool> IsModelDownloaded()
    {
        const string url = "http://localhost:11434/api/tags";

        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        var json = JsonNode.Parse(response.Content.ReadAsStringAsync().Result);
        return json?["models"]?.AsArray().Any(m => m?["name"]?.ToString() == "llama3.2:latest" ||
            m?["name"]?.ToString() == "llama3.2") ?? false;
    }

    public async Task<string> GetModelParamNum(string modelName)
    {
        const string url = "http://localhost:11434/api/show";

        var payload = new
        {
            model = modelName
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var response = await client.PostAsync(url, content);
        var json = JsonNode.Parse(response.Content.ReadAsStringAsync().Result);
        return ":" + json?["details"]?["parameter_size"]?.ToString().ToLower();
    }

    public async IAsyncEnumerable<OllamaResponse> GenerateMessage(List<Message> messageHistory)
    {
        const string url = "http://localhost:11434/api/chat";

        var chatRequest = new ChatRequest(messageHistory);
        var jsonPayload = chatRequest.ToJson();

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        using var client = new HttpClient();
        using var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                OllamaResponse? json = null;
                try
                {
                    json = JsonSerializer.Deserialize<OllamaResponse>(line);
                }
                catch (JsonException)
                {
                    // insert error handling here
                }
                if (json != null)
                {
                    yield return json;
                }
            }
        }
    }

    public async IAsyncEnumerable<DownloadResponse> PullModel(string modelName)
    {
        const string url = "http://localhost:11434/api/pull";

        var payload = new
        {
            model = modelName,
            stream = true
        };

        var jsonPayload = JsonSerializer.Serialize(payload);

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        using var client = new HttpClient();
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                DownloadResponse? json = null;
                try
                {
                    json = JsonSerializer.Deserialize<DownloadResponse>(line);
                }
                catch (JsonException)
                {
                    // Console.WriteLine("error json no good serialize");
                }

                if (json != null)
                {
                    yield return json;
                }
            }
        }
    }
}