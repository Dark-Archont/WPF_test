using System.IO;
using System.Text.Json;

namespace SmartMealWpf.Services;

/// <summary>
/// Persists user-entered comments to a local JSON file alongside the executable.
/// Comments are not part of the OS environment variable system, so they require
/// a separate lightweight store.
/// </summary>
public sealed class CommentStoreService
{
    private static readonly string FilePath = Path.Combine(
        AppContext.BaseDirectory, "comment-store.json");

    public Dictionary<string, string> Load()
    {
        if (!File.Exists(FilePath))
            return new Dictionary<string, string>();

        try
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                   ?? new Dictionary<string, string>();
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    public void Save(Dictionary<string, string> comments)
    {
        var json = JsonSerializer.Serialize(comments, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
