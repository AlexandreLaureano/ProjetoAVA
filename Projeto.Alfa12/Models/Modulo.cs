using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Projeto.Alfa12.Models
{
    public class Modulo
    {
        public int Id { get; set; }
        public string Nome{ get; set; }
        public string Descricao{ get; set; }
        public string Url { get; set; } //Ferramenta, RespAuto
        public byte[] Arquivo { get; set; } // ConteudoPdf
        public string Texto { get; set; } //ConteudoTexto, Resps
        public string Resposta { get; set; } //Resps
        public TipoMod Tipo { get; set; }
        public int MaxPonto { get; set; }

        public ICollection<Pontuacao> Pontuacao { get; set; }

        public int? TurmaId { get; set; }
        public Turma Turma { get; set; }
    }
    public enum TipoMod
    {
        Ferramenta = 1,
        ConteudoPdf,
        ConteudoTexto,
        RespAuto,
        RespManual
    }
}
