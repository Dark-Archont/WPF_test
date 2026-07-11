namespace SmartMealWpf.Services;

public interface IAppLogger
{
    void LogVariableChanged(string name, string oldValue, string newValue);
    void LogInformation(string message);
    void LogError(string message, Exception? exception = null);
}
