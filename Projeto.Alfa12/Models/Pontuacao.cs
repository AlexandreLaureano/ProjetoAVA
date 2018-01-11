using System;


namespace Projeto.Alfa12.Models
{
    public class Pontuacao
    {
        public int Id{ get; set; }
        public int AlunoId { get; set; }
        public int TurmaId { get; set; }
        public int ModuloId { get; set; }
        public int Pontos { get; set; }
        public DateTime Data{ get; set; }

        //relação com aluno gamification
    }
}
