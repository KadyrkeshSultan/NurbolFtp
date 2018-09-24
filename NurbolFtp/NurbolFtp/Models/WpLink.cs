using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NurbolFtp.Models
{
    public class WpLink
    {
        [Required(ErrorMessage ="Введите название аккаунта")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Введите номер телефона")]
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string Text { get; set; }
        public int Theme { get; set; }

        [Required(ErrorMessage ="Введите пароль")]
        public string Password { get; set; }

        public IFormFile File { get; set; }
    }
}
