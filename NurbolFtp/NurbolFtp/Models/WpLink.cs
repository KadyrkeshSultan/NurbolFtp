using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace NurbolFtp.Models
{
    public class WpLink
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string Text { get; set; }
        public int Theme { get; set; }

        public IFormFile File { get; set; }
    }
}
