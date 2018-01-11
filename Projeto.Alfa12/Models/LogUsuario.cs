using System;


namespace Projeto.Alfa12.Models
{
    public class LogUsuario
    {
        public int Id { get; set; }
        public string Acao { get; set; }
        public DateTime Data{ get; set; }
        public int UsuarioId { get; set; }

        public ApplicationUser Usuario { get; set; }
    }
}
