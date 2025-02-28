using System.Collections.Generic;
using System.Threading.Tasks;

using SmartGenealogy.Models;

namespace SmartGenealogy.Services;

public interface IOllamaService
{
    Task<GeneratedMessage?> GenerateMessage(string prompt);

    Task<List<OllamaModel>?> GetOllamaModels();
}