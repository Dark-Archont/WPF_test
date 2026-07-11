using Serilog;

namespace SmartMealWpf.Services;

public sealed class SerilogAppLogger : IAppLogger
{
    private readonly ILogger _logger;

    public SerilogAppLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void LogVariableChanged(string name, string oldValue, string newValue)
    {
        _logger.Information(
            "Variable '{Name}' changed: '{OldValue}' -> '{NewValue}'",
            name, oldValue, newValue);
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogError(string message, Exception? exception = null)
    {
        _logger.Error(exception, message);
    }
}
