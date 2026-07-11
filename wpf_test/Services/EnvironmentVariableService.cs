namespace SmartMealWpf.Services;

/// <summary>
/// Reads and writes User-level environment variables.
/// User-level variables are persisted in the Windows registry and
/// survive OS reboots without requiring administrator privileges.
/// </summary>
public sealed class EnvironmentVariableService : IEnvironmentVariableService
{
    public string? Read(string name)
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
    }

    public void Write(string name, string value)
    {
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.User);
    }
}
