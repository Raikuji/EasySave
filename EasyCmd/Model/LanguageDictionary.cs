using System;
using System.Collections.Generic;
using System.Text.Json;

/// <summary>
/// Class to manage language dictionary
/// </summary>
public class LanguageDictionary
{
    private Dictionary<string, string> _dictionary;

    /// <summary>
    /// Constructor
    /// </summary>
    public LanguageDictionary()
    {
        _dictionary = new Dictionary<string, string>();
    }

    /// <summary>
    /// Get a string from the dictionary
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetString(string key)
    {
        return _dictionary.ContainsKey(key) ? _dictionary[key] : key;
    }

    /// <summary>
    /// Add a string to the dictionary
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddString(string key, string value)
    {
        _dictionary.Add(key, value);
    }

    /// <summary>
    /// Load language from a json file
    /// </summary>
    /// <param name="jsonFilePath"></param>
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
