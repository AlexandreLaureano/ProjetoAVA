

namespace Projeto.Alfa12.Models
{
    public class AlunoConquista
    {
        public int ConquistaId{ get; set; }
        public int AlunoId { get; set; }

        public Conquista Conquista{ get; set; }
        public AlunoGamification Aluno { get; set; }
    }
}
