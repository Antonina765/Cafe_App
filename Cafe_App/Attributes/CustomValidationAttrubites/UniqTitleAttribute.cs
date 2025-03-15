using Cafe.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;

namespace Cafe_App.Attributes.CustomValidationAttrubites
{
    public class UniqTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, 
            ValidationContext validationContext)
        {
            var title = value as string;
            if (title == null)
            {
                return new ValidationResult("Not a string");
            }

            var repository = validationContext.GetRequiredService<ICafeRepository<CafeData>>();
            if (!repository.IsTitleUniq(title))
            {
                return new ValidationResult("Not uniq title");
            }

            return ValidationResult.Success;
        }
    }
}
