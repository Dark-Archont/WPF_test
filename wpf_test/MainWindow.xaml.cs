using System.Windows;
using System.Windows.Controls;
using SmartMealWpf.Models;
using SmartMealWpf.ViewModels;

namespace SmartMealWpf;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
        _viewModel.LoadVariables();
    }

    private void EnvVarGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.EditAction == DataGridEditAction.Commit
            && e.Row.Item is EnvironmentVariableItem item)
        {
            // Dispatch to avoid reentrancy during DataGrid commit cycle.
            Dispatcher.InvokeAsync(() => _viewModel.SaveItemCommand.Execute(item));
        }
    }
}
