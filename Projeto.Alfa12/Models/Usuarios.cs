using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Projeto.Alfa12.Models
{
    public class Aluno : ApplicationUser
    {
        public int Ra { get; set; }

        private ICollection<AlunoTurma> Turmas { get; } = new List<AlunoTurma>();

        public AlunoGamification AlunoGamification { get; set; }

        //1 pra 1 com Aluno Gamification

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
