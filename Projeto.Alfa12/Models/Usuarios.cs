using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Projeto.Alfa12.Models
{
    public class Aluno : ApplicationUser
    {
        public int Ra { get; set; }
        public int? PontoGeral { get; set; }

        [NotMapped]
        public int PontoParcial { get; set; }

        private ICollection<AlunoTurma> Turmas { get; } = new List<AlunoTurma>();


        public ICollection<Pontuacao> Pontuacao { get; set; }
        public ICollection<AlunoConquista> Conquistas { get; set; }


        [NotMapped]
        public IEnumerable<Turma> ITurma => Turmas.Select(e => e.Turma);
    }

    public class Professor : ApplicationUser
    {
        public bool Autenticado{ get; set; }

        public ICollection<Turma> Turmas { get; set; }
    }

    public class Coordenador : ApplicationUser
    {

    }
}
