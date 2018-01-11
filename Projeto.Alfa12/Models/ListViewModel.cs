using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Models
{
    [NotMapped]
    public class ListViewModel
    {
    }

    [NotMapped]
    public class AlunoViewList
    {

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nome")]
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Identificador do Aluno")]
        public int Ra { get; set; }

        
    }

    [NotMapped]
    public class TurmaEditModel
    {
        public Turma Turma { get; set; }
        [Display(Name = "Membros")]
        public IEnumerable<Aluno> Members { get; set; }
        [Display(Name = "Não membros")]
        public IEnumerable<Aluno> NonMembers { get; set; }

    }

    [NotMapped]
    public class TurmaEditModulo
    {
        public Turma Turma { get; set; }
        [Display(Name = "Possui")]
        public IEnumerable<Modulo> Members { get; set; }
        [Display(Name = "Não possui")]
        public IEnumerable<Modulo> NonMembers { get; set; }

    }

    [NotMapped]
    public class TurmaModificationModel
    {
        [Required]
        public string TurmaName { get; set; }
        public int TurmaId { get; set; }
        public int[] IdsToAdd { get; set; }
        public int[] IdsToDelete { get; set; }
    }
}
