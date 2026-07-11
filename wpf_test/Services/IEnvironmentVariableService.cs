namespace SmartMealWpf.Services;

public interface IEnvironmentVariableService
{
    /// <summary>
    /// Reads the current value of the specified environment variable.
    /// Returns null if the variable is not set.
    /// </summary>
    string? Read(string name);

    /// <summary>
    /// Writes the value to the User-level environment variable store,
    /// making it persistent across OS reboots and accessible to other processes.
    /// </summary>
    void Write(string name, string value);
}
