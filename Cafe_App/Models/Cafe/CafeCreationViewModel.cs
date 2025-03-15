using System.ComponentModel.DataAnnotations;
using Cafe_App.Attributes.CustomValidationAttrubites;

namespace Cafe_App.Models.Cafe;

public class CafeCreationViewModel
{ 
    [UniqTitle]
    public string Title { get; set; }
    
    /*[IsUrl(
        ErrorMessageResourceType = typeof(Localizations.Cafe))]
        //ErrorMessageResourceName = nameof(Cafe.ValidationMessage_IsNotUrl))]*/

    public string? ImageSrc { get; set; }
    
    //[MaxFileSize(52428800)]
    /*[FileExtensions(Extensions = "jpg,jpeg,png", 
        ErrorMessageResourceType = typeof(Localizations.Cafe), 
        ErrorMessageResourceName = nameof(Localizations.Cafe.ValidationMessage_UploadFile))]*/
    //[FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Please upload a valid image file (jpg, jpeg, png).")]
    public IFormFile? ImageFile { get; set; }
    
    public double Rang { get; set; }
    
    public string Address { get; set; }
}