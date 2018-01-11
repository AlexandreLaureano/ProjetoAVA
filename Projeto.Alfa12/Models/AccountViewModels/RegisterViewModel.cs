using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme sua senha")]
        [Compare("Password", ErrorMessage = "Senhas inseridas não conferem")]
        public string ConfirmPassword { get; set; }
    }

    public class AlunoRegister
    {
        [Required(ErrorMessage = "É necessário inserir um email.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É necessário inserir uma senha.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme sua senha")]
        [Compare("Password", ErrorMessage = "Senhas inseridas não conferem")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "É necessário informar seu nome.")]
        [StringLength(50)]
        [Display(Name = "Nome")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "É necessário informar seu sobrenome.")]
        [StringLength(50)]
        [Display(Name = "Sobrenome")]
        public string Lastname { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "É necessário informar a data de nascimento.")]
        public DateTime Birthday { get; set; }
    }
}
