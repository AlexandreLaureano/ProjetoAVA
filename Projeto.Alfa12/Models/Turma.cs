using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Projeto.Alfa12.Models
{
    public class Turma
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataCriacao{ get; set; }
        public string Descricao{ get; set; }
        public string AreaConhecimento{ get; set; }
        public string ChaveAcesso{ get; set; }
        public int? MaxPonto{ get; set; }

        public int? ProfessorId { get; set; }
        public Professor Professor { get; set; }

        public ICollection<Turma> Modulos { get; set; }

        private ICollection<AlunoTurma> Alunos { get; } = new List<AlunoTurma>();
        
       [NotMapped]
       public IEnumerable<Aluno> IAluno => Alunos.Select(e => e.Aluno);
       
    }
}
