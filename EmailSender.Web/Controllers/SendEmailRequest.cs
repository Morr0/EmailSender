using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmailSender.Web.Controllers
{
    public class SendEmailRequest : IValidatableObject
    {
        [Required, MinLength(1)] public string Subject { get; set; }
        [Required, MinLength(1)] public string Template { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Template.Contains("{0}")) yield break;

            yield return new ValidationResult("Please include at least one {0} to substitute for",
                new[] {nameof(Template)});
        }
    }
}