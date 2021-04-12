using System.ComponentModel.DataAnnotations;

namespace EmailSender.Web.Dtos
{
    public class Destination
    {
        [Required, MinLength(1)] public string Name { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
    }
}