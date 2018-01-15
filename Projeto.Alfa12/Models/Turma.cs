using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Projeto.Alfa12.Models
{
    public class Turma
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        [Display(Name ="Data da Criação")]
        public DateTime DataCriacao{ get; set; }
        [Display(Name ="Descrição")]
        public string Descricao{ get; set; }
        [Display(Name = "Área de Conhecimento")]
        public string AreaConhecimento{ get; set; }
        [Display(Name = "Chave de Acesso")]
        public string ChaveAcesso{ get; set; }
        [Display(Name = "Pontos")]
        public int? MaxPonto{ get; set; }

        public int? ProfessorId { get; set; }
        public Professor Professor { get; set; }

        public ICollection<Turma> Modulos { get; set; }

        private ICollection<AlunoTurma> Alunos { get; } = new List<AlunoTurma>();
        
       [NotMapped]
       public IEnumerable<Aluno> IAluno => Alunos.Select(e => e.Aluno);
       
    }
}
