using System;
using System.Collections.Generic;

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
		_dictionary[key] = value;
	}
}

// Class representing the language manager using the Singleton pattern.
public class Language
{
	// Unique instance of the class (Singleton)
	private static Language _instance;

	// Associated language dictionary
	private LanguageDictionary _languageDictionary;

	// Private constructor to prevent external instantiation (Singleton).
	private Language() { }

	// Returns the unique instance of the Language class.
	// returns: Unique instance of Language
	public static Language GetInstance()
	{
		if (_instance == null)
		{
			_instance = new Language();
		}
		return _instance;
	}

	// Sets the language dictionary to be used.
	// param languageDictionary: Instance of the language dictionary
	public void SetLanguage(LanguageDictionary language)
	{
		_languageDictionary = language;
	}
}