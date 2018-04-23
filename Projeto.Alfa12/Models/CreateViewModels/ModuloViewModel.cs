using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Models.CreateViewModels
{
    public class ModuloViewModel
    {
        [Required(ErrorMessage = "É necessário inserir um nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É necessário inserir uma descrição")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public string Url { get; set; } 

        [Display(Name ="Turma")]
        public int TurmaId { get; set; }

        public IFormFile Arquivo { get; set; }
        public string Texto { get; set; }
        public string Resposta { get; set; }
        public int Tipo { get; set; }
    }
}
