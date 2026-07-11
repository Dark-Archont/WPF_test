using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using SmartMealWpf.Models;
using SmartMealWpf.Services;

namespace SmartMealWpf.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IEnvironmentVariableService _envService;
    private readonly IAppLogger _logger;
    private readonly CommentStoreService _commentStore;
    private readonly IReadOnlyList<string> _variableNames;
    private readonly IReadOnlyDictionary<string, string> _defaults;

    [ObservableProperty]
    private ObservableCollection<EnvironmentVariableItem> _items = new();

    public MainViewModel(
        IEnvironmentVariableService envService,
        IAppLogger logger,
        CommentStoreService commentStore,
        IConfiguration configuration)
    {
        _envService = envService;
        _logger = logger;
        _commentStore = commentStore;

        _variableNames = configuration
            .GetSection("EnvironmentVariables:Names")
            .Get<List<string>>() ?? new List<string>();

        _defaults = configuration
            .GetSection("EnvironmentVariables:Defaults")
            .Get<Dictionary<string, string>>() ?? new Dictionary<string, string>();
    }

    public void LoadVariables()
    {
        var comments = _commentStore.Load();
        var loaded = new ObservableCollection<EnvironmentVariableItem>();

        foreach (var name in _variableNames)
        {
            var existingValue = _envService.Read(name);

            // If the variable doesn't exist yet, initialize it with the configured default.
            if (existingValue is null)
            {
                var defaultValue = _defaults.TryGetValue(name, out var def) ? def : string.Empty;
                _envService.Write(name, defaultValue);
                _logger.LogInformation($"Variable '{name}' not found. Initialized with default value.");
                existingValue = defaultValue;
            }

            loaded.Add(new EnvironmentVariableItem
            {
                Name = name,
                Value = existingValue,
                Comment = comments.TryGetValue(name, out var c) ? c : string.Empty
            });
        }

        Items = loaded;
        _logger.LogInformation($"Loaded {loaded.Count} environment variable(s) on startup.");
    }

    [RelayCommand]
    private void SaveItem(EnvironmentVariableItem? item)
    {
        if (item is null) return;

        var oldValue = _envService.Read(item.Name) ?? string.Empty;

        if (oldValue == item.Value)
        {
            // Value unchanged — only save comment if it differs.
            SaveComments();
            return;
        }

        _envService.Write(item.Name, item.Value);
        _logger.LogVariableChanged(item.Name, oldValue, item.Value);

        SaveComments();
    }

    private void SaveComments()
    {
        var comments = Items.ToDictionary(i => i.Name, i => i.Comment);
        _commentStore.Save(comments);
    }
}
