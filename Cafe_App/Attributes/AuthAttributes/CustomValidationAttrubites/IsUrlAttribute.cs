using System.ComponentModel.DataAnnotations;
using System.Resources;
using Cafe_App.Localizations;

namespace Cafe_App.Attributes.CustomValidationAttrubites
{
    public class IsUrlAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            if (!string.IsNullOrEmpty(ErrorMessage)) {
                return ErrorMessage;
            }

            if (ErrorMessageResourceType is not null && !string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                var resourceManager = new ResourceManager(ErrorMessageResourceType.FullName,
                    ErrorMessageResourceType.Assembly);
                return resourceManager.GetString(ErrorMessageResourceName);
            }

            return "Не правильный урл";
        }

        public override bool IsValid(object? value)
        {
            var url = value as string;
            if (url == null)
            {
                return false;
            }

            if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("data:image"))
            {
                return false;
            }

            return true;
        }
    }
}
