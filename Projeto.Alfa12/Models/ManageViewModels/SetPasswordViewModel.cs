using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Models.ManageViewModels
{
    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = "É necessário inserir uma senha.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a nova senha")]
        [Compare("NewPassword", ErrorMessage = "Senhas inseridas não conferem.")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
