using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartGenealogy.Services;

public class OllamaService
{
    private Process? _ollamaProcess;
    private string OllamaPath { get; set; }

    public delegate void ServiceStatusChangedHandler(ServiceStatus status, string? message);

    public event ServiceStatusChangedHandler? ServiceStatusChanged;

    public OllamaService()
    {
        OllamaPath = "";
    }

    private async Task Start()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            OllamaPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"Programs\Ollama\ollama");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            OllamaPath = @"/usr/local/bin/ollama";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            OnServiceStatusChanged(
                ServiceStatus.Failed,
                "Not supported");
            return;
        }

        var processCount = OllamaProcessCount();
        switch (processCount)
        {
            case 0:
                break;

            case 1:
                OnServiceStatusChanged(
                    ServiceStatus.Running,
                    "Already running");
                return;

            case 2:
                OnServiceStatusChanged(
                    ServiceStatus.Failed,
                    "MultipleInstances");
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
        catch (Exception ex)
        {
            OnServiceStatusChanged(
                ServiceStatus.Failed,
                "Ollama failed");
            return;
        }

        if (_ollamaProcess == null)
        {
            OnServiceStatusChanged(
                ServiceStatus.Failed,
                "Ollama not installed");
            return;
        }

        var isServerRunning = await IsOllamaServerRunning();
        if (isServerRunning)
        {
            OnServiceStatusChanged(
                ServiceStatus.Running,
                "Ollama started");
        }
        else
        {
            OnServiceStatusChanged(
                ServiceStatus.Failed,
                "Ollama connection failed");
        }
    }

    private uint OllamaProcessCount()
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
        if (_ollamaProcess == null || _ollamaProcess.HasExited) return;
        _ollamaProcess.Kill();
        _ollamaProcess.Dispose();
    }

    private void OnServiceStatusChanged(ServiceStatus status, string? message = null)
    {
        ServiceStatusChanged?.Invoke(status, message);
    }

    public async Task<GeneratedMessage?> GenerateMessage(string prompt)
    {
        const string url = "http://localhost:11434/api/generate";
        GeneratedMessage? generatedMessage = null;

        var data = new
        {
            model = "llama3.2",
            prompt = prompt,
            stream = false
        };

        using var client = new HttpClient();
        var jsonData = JsonSerializer.Serialize(data);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);

                if (jsonResponse.TryGetProperty("response", out var answer) &&
                    jsonResponse.TryGetProperty("eval_count", out var evalCount) &&
                    jsonResponse.TryGetProperty("eval_duration", out var evalDuration))
                {
                    generatedMessage = new GeneratedMessage(answer.GetString() ?? string.Empty, (double)evalCount.GetInt32() / evalDuration.GetInt64() * 1e9);
                }
            }
            else
            {
                generatedMessage = new GeneratedMessage("Exception occurred, please restart the application. Error message: " + response.StatusCode, 0);
            }
        }
        catch (Exception ex)
        {
            generatedMessage = new GeneratedMessage("Exception occurred, please restart the application: " + ex.Message, 0);
        }

        return generatedMessage;
    }
}

public enum ServiceStatus
{
    Running,
    Failed
}