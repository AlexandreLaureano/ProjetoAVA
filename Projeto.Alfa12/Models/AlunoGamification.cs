
using System.Collections.Generic;


namespace Projeto.Alfa12.Models
{
    public class AlunoGamification
    {
     
        public int AlunoId { get; set; }
        public int PontoGeral { get; set; }


        public ICollection<Pontuacao> Pontuacao { get; set; }
        public ICollection<AlunoConquista> Conquistas { get; set; }

        public Aluno Aluno { get; set; }
        //1 pra 1 com aluno
    }
}
