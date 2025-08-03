using System.Text.Json;
using VAHub.Logging;

namespace VAHub.Managers;

public class OptionsManager
{
    private JsonDocument? _document;

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="JsonException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    public OptionsManager(string jsonFilePath, bool fileOptional = false)
    {
        if (string.IsNullOrEmpty(jsonFilePath))
        {
            Logger.Error("Путь к файлу кофигурации не может быть пустым");
            throw new ArgumentNullException(nameof(jsonFilePath));
        }

        if (!fileOptional && !File.Exists(jsonFilePath))
        {
            Logger.Error($"Файл конфигурации '{jsonFilePath}' не найден");
            throw new FileNotFoundException($"File '{jsonFilePath}' not found.");
        }
        else if (File.Exists(jsonFilePath))
        {
            string text = File.ReadAllText(jsonFilePath);
            _document = JsonDocument.Parse(text);
        }
    }

    /// <summary>
    /// Gets class by name or returns new T if not found.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="JsonException"></exception>
    public T Get<T>(string name) where T : new()
    {
        if (string.IsNullOrEmpty(name)) 
            throw new ArgumentNullException(nameof(name));

        if(_document != null && _document.RootElement.TryGetProperty(name, out JsonElement element))
        {
            T? value = element.Deserialize<T>();

            if (value != null)
                return value;
        }
        return new();
    }
}