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

    [NotMapped]
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages =>
        (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }

    [NotMapped]
    public class ProductsListViewModel
    {
        public IEnumerable<Turma> Itens { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class LogListViewModel
    {
        public IEnumerable<LogUsuario> Logs { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }

    [NotMapped]
    public class RankViewModel
    {
        public int Nome { get; set; }
        public int? Ponto { get; set; }
    }

    [NotMapped]
    public class RankListViewModel
    {
        public IEnumerable<Turma> Turmas { get; set; }
        public int Op { get; set; }
        public int IdTurma { get; set; }
    }

    [NotMapped]
    public class ModuloPontuacaoViewModel
    {
        public Modulo modulo { get; set; }
        public Pontuacao pontuacao { get; set; }
    }

    [NotMapped]
    public class AlunoPontos
    {
        public Modulo modulo { get; set; }
        public Pontuacao pontuacao { get; set; }
    }


}
