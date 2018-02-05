using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "É necessário inserir um email.")]
        [EmailAddress(ErrorMessage ="Insira um email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É necessário inserir uma senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me?")]
        public bool RememberMe { get; set; }
    }
}
