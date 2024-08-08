using System.Text.Json;

namespace DotnetAppSettings.Formatters;

internal class JsonObjectEnvironmentOutputFormatter : IOutputFormatter
{
  /// <inheritdoc />
  public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
  {
    var content = settings.OrderBy(p => p.Name).ToDictionary(s => s.Name, s => (object?)s.Value);
    var result = TransformToNestedObject(content);
    return JsonSerializer.SerializeAsync(stream, result.ToDictionary(s => s.Key, s => s.Value),
      new JsonSerializerOptions
      {
        WriteIndented = true
      });
  }

  private static Dictionary<string, object> TransformToNestedObject(Dictionary<string, object?> data)
  {
    var result = new Dictionary<string, object>();

    foreach (var (key, value) in data)
    {
      var keys = key.Split("__");

      AddToNestedObject(result, keys, value);
    }

    return result;
  }

  private static void AddToNestedObject(Dictionary<string, object>? currentLevel, string[] keys, object? value)
  {
    for (var i = 0; i < keys.Length; i++)
    {
      var key = keys[i];
      if (!currentLevel!.ContainsKey(key))
      {
        if (i == keys.Length - 1)
        {
          currentLevel[key] = value!; // Assign value to the deepest key
        }
        else
        {
          currentLevel[key] = new Dictionary<string, object>();
          currentLevel = (Dictionary<string, object>)currentLevel[key]; // Move to the next level
        }
      }
      else
      {
        if (currentLevel[key] is Dictionary<string, object>)
        {
          currentLevel = (Dictionary<string, object>)currentLevel[key];
        }
        else
        {
          throw new InvalidOperationException($"Conflict at {key}");
        }
      }
    }
  }
}