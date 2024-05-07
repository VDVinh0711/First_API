using System.ComponentModel.DataAnnotations;

namespace API_EF.Models.DTOs
{
    public class UserRegistertrationRequestDto
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }    

    }
}
