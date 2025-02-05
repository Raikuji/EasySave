using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    // Class representing the language manager using the Singleton pattern.
    internal class Language
    {
        // Unique instance of the class (Singleton)
        private static Language? _instance;

        // Associated language dictionary
        private LanguageDictionary _languageDictionary;

        // Private constructor to prevent external instantiation (Singleton).
        private Language()
        {
            _languageDictionary = new LanguageDictionary();
        }

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
        
        public string GetString(string key)
        {
            return _languageDictionary.GetString(key);
        }
    }
}
