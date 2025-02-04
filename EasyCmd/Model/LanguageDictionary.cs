using System;
using System.Collections.Generic;
using System.Text.Json;

// Class representing a language dictionary that stores translations as key/value pairs.
public class LanguageDictionary
{
    // Private dictionary containing translations
    private Dictionary<string, string> _dictionary;

    // Constructor that initializes an empty dictionary.
    public LanguageDictionary()
    {
        _dictionary = new Dictionary<string, string>();
    }

    // Retrieves the translation associated with a given key.
    // If the key does not exist, it returns the key itself.
    // param key: Translation key
    // returns: Translated value if available, otherwise the key
    public string GetString(string key)
    {
        return _dictionary.ContainsKey(key) ? _dictionary[key] : key;
    }

    // Adds a new translation or updates an existing one.
    // param key: Translation key
    // param value: Translated value
    public void AddString(string key, string value)
    {
        _dictionary.Add(key, value);
    }

    public void LoadLanguage(string jsonFilePath)
    {
        string stringLang = File.ReadAllText(jsonFilePath);
        foreach (JsonElement jsonElement in JsonDocument.Parse(stringLang).RootElement.EnumerateArray())
        {
            foreach (JsonProperty jsonProperty in jsonElement.EnumerateObject())
            {
                AddString(jsonProperty.Name, jsonProperty.Value.GetRawText().Replace("\"", "").Replace("\\n", Environment.NewLine));
            }
        }
    }
}
