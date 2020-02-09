using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dtos
{
    public class UserForLoginDto
    {
        [Required(ErrorMessage = "The Username field is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(8,MinimumLength =4,ErrorMessage ="You must specify password between 4 and 8 characters.")]
        public string Password { get; set; }
    }
}
