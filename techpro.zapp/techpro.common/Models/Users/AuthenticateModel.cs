using System.ComponentModel.DataAnnotations;

namespace techpro.common.Models.Users
{
    public class AuthenticateModel
    {
        [Required]

        public string Username { get; set; }

        [Required]

        public string Password { get; set; }
    }
}