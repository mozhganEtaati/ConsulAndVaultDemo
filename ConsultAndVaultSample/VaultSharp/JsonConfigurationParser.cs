using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace ConsultAndVaultSample.VaultSharp;
public class JsonConfigurationParser
{
    private readonly Dictionary<string, string?> _data = new(StringComparer.OrdinalIgnoreCase);
    private readonly Stack<string> _paths = new();

    public Dictionary<string, string?> Parse(string jsonString)
    {
        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        using (JsonDocument doc = JsonDocument.Parse(jsonString, jsonDocumentOptions))
        {
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new ArgumentException(jsonString);
            }
            ParseObjectElement(doc.RootElement);
        }

        return _data;
    }

    private void ParseObjectElement(JsonElement element)
    {
        var isEmpty = true;

        foreach (JsonProperty property in element.EnumerateObject())
        {
            isEmpty = false;
            PushConfigPath(property.Name);
            ParseJsonValue(property.Value);
            PopConfigPath();
        }

        HandleEmptyElement(isEmpty);
    }

    private void ParseArrayElement(JsonElement element)
    {
        int index = 0;

        foreach (JsonElement arrayElement in element.EnumerateArray())
        {
            PushConfigPath(index.ToString(CultureInfo.CurrentCulture));
            ParseJsonValue(arrayElement);
            PopConfigPath();
            index++;
        }

        HandleEmptyElement(isEmpty: index == 0);
    }

    private void HandleEmptyElement(bool isEmpty)
    {
        if (isEmpty && _paths.Count > 0)
        {
            _data[_paths.Peek()] = null;
        }
    }

    private void ParseJsonValue(JsonElement value)
    {
        Debug.Assert(_paths.Count > 0);

        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                ParseObjectElement(value);
                break;

            case JsonValueKind.Array:
                ParseArrayElement(value);
                break;

            case JsonValueKind.Number:
            case JsonValueKind.String:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
                string key = _paths.Peek();
                if (_data.ContainsKey(key))
                {
                    throw new ArgumentException(nameof(value.ValueKind));
                }
                _data[key] = value.ToString();
                break;

            default:
                throw new ArgumentException(nameof(value.ValueKind));
        }
    }

    private void PushConfigPath(string context) =>
        _paths.Push(_paths.Count > 0 ?
            _paths.Peek() + ConfigurationPath.KeyDelimiter + context :
            context);

    private void PopConfigPath() => _paths.Pop();
}

