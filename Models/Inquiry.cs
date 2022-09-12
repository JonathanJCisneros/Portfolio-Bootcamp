#pragma warning disable CS8618
namespace Portfolio.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inquiry
{
    [Key]
    public int InquiryId {get; set;}

    [Required(ErrorMessage = "is requried")]
    [MinLength(2, ErrorMessage = "must be at least 2 characters long")]
    [Display(Name = "Name")]
    public string Name {get; set;}

    [Required(ErrorMessage = "is required")]
    [EmailAddress(ErrorMessage = "must be a valid Email address")]
    [Display(Name = "Email")]
    public string Email {get; set;}

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Inquiry Type")]
    public string InquiryType {get; set;}

    [Required(ErrorMessage = "is required")]
    [MinLength(4, ErrorMessage = "details must be at least 4 characters long")]
    [Display(Name = "Details")]
    public string Details {get; set;}
    public string Status {get; set;} = "Unresolved";
    public DateTime CreatedAt {get; set;} = DateTime.Now;
    public DateTime UpdatedAt {get; set;} = DateTime.Now;
}