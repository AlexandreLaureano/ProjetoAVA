using Microsoft.AspNetCore.Mvc;
using Projeto.Alfa12.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Components
{
    public class AlunoPonto : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AlunoPonto(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int aluno, int? turma)
        {

            // var pontos = _context.Pontuacoes.FromSql
            //     context.Blogs.SqlQuery("SELECT * FROM dbo.Blogs").ToList();
            var ponto = _context.Pontuacoes.Where(x => x.AlunoId == aluno).Sum(x => x.Pontos);

            var pontoturma = _context.Pontuacoes.Where(x => x.AlunoId == aluno || x.TurmaId == turma).Sum(x => x.Pontos);

            return View("APonto", ponto);


        }
    }
}
