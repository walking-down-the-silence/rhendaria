using System.ComponentModel.DataAnnotations;

namespace Rhendaria.Web.Models
{
    public class JoinGameViewModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(15)]
        public string Username { get; set; }
    }
}
