using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCmd.Model
{
    /// <summary>
    /// Class that represents the language used in the application.
    /// </summary>
    internal class Language
    {
        private static Language? _instance;
        private LanguageDictionary _languageDictionary;

        /// <summary>
        /// Constructor of the Language class.
        /// </summary>
        private Language()
        {
            _languageDictionary = new LanguageDictionary();
        }

        /// <summary>
        /// Returns the instance of the Language class.
        /// </summary>
        /// <returns></returns>
        public static Language GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Language();
            }
            return _instance;
        }

        /// <summary>
        /// Sets the language of the application.
        /// </summary>
        /// <param name="language"></param>
        public void SetLanguage(LanguageDictionary language)
        {
            _languageDictionary = language; 
        }

        /// <summary>
        /// Returns the string value of the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            return _languageDictionary.GetString(key);
        }
    }
}
