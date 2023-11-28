using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace firsttime.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        [StringLength(30, ErrorMessage = "Имя должно быть менее 30 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Email { get; set; }

        public User() { }
        public User(string login, string email, string password)
        {
            Login = login;
            Email = email;
            Password = password;
        }
    }
}
