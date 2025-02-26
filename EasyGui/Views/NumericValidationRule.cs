using System.Globalization;
using System.Windows.Controls;

namespace EasyGui.ValidationRules
{
    public class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (long.TryParse(value.ToString(), out long result) && result > 0)
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Veuillez entrer une valeur valide pour la taille maximale des fichiers.");
        }
    }
}
