using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Projeto.Alfa12.Models.CreateViewModels
{
    public class TurmaViewModel
    {
        [Required(ErrorMessage ="É necessário inserir um nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É necessário inserir uma descrição")]
        [Display(Name ="Descrição")]
        public string Descricao{ get; set; }

        [Display(Name = "Área de Conhecimento")]
        public string AreaConhecimento{ get; set; }

        [Display(Name = "Chave de Acesso")]
        public string ChaveAcesso{ get; set; }
       
    }
}
