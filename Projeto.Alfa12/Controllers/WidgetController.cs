using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Data;

namespace Projeto.Alfa12.Controllers
{
    public class WidgetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WidgetController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Ranking(int id)
        {
            var lista = _context.Pontuacoes.Where(x => x.TurmaId == id);
            var t = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(y => y.Id == id);
            var alunos = t.IAluno.Distinct();
            foreach (var a in alunos)
            {
                int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
            }

            return View("Ranking", alunos.OrderByDescending(x => x.PontoParcial));

        }

        public IActionResult Resposta(int id)
        {
            return View();
        }
    }
}