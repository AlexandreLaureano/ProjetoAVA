using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Projeto.Alfa12.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {

        public ApplicationUser()
         : base()
        {
        }

        public ApplicationUser(string userName, string firstName, string lastName, DateTime birthDay, bool ativo)
            : base(userName)
        {
            base.Email = userName;

            this.Nome = firstName;
            this.Sobrenome = lastName;
            this.DataNascimento = birthDay;
            this.Ativo = ativo;

        }


        [Required]
        [StringLength(50)]
        public string Nome { get; set; }
        [Required]
        [StringLength(50)]
        public string Sobrenome { get; set; }

        //public string Discriminator{ get; set; }
        public bool Ativo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        public ICollection<LogUsuario> Logs { get; set; }
        public ICollection<Mensagem> Origem { get; set; }
        public ICollection<Mensagem> Destino { get; set; }


        public string FullName => $"{this.Nome} {this.Sobrenome}";
    }

    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName)
        {
            base.Name = roleName;
        }
    }

   
}
