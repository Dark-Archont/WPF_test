using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartMealWpf.Models;

/// <summary>
/// Represents a single environment variable row displayed in the DataGrid.
/// </summary>
public partial class EnvironmentVariableItem : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _value = string.Empty;

    [ObservableProperty]
    private string _comment = string.Empty;
}
