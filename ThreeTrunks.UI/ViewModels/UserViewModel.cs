using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ThreeTrunks.UI.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [DisplayName("Логин")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}