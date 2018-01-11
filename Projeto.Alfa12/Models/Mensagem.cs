using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Alfa12.Models
{
    public class Mensagem
    {
        public int Id{ get; set; }
        public int OrigemFK{ get; set; }
        public int DestinoFK { get; set; }
        public string Texto{ get; set; }
        public DateTime Data { get; set; }

        [ForeignKey("OrigemFK")]
        public ApplicationUser Origem { get; set; }
        [ForeignKey("DestinoFK")]
        public ApplicationUser Destino { get; set; }


    }
}
