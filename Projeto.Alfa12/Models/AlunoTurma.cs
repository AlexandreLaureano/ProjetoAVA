using System;


namespace Projeto.Alfa12.Models
{
    public class AlunoTurma
    {
        public int Id { get; set; }
        public bool Ativo{ get; set; }
        public DateTime DataIngressao{ get; set; }

        public int TurmaId { get; set; }
        public int AlunoId { get; set; }

        public Aluno Aluno { get; set; }
        public Turma Turma { get; set; }
    }
}
