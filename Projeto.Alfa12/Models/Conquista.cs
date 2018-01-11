
using System.Collections.Generic;


namespace Projeto.Alfa12.Models
{
    public class Conquista
    {
        public int Id { get; set; }
        public string Nome{ get; set; }
        public string Requisito{ get; set; }

        public ICollection<AlunoConquista> Alunos { get; set; }
    }
}
