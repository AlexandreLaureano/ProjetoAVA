using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;

namespace Projeto.Alfa12.Controllers
{
    public class WidgetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public WidgetController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Resposta(int? id)
        {
            var user = _userManager.GetUserAsync(User);
            if (user == null)
            {
               return RedirectToAction(nameof(Login));
            }
            var aluno = await user;
            var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == modulo.TurmaId);

            if (turma.IAluno.Contains(aluno))
            {
                if (id == null)
                {
                    return NotFound();
                }


                if (modulo == null)
                {
                    return NotFound();
                }

                return View("Resposta", modulo);
            }
            else
            {
                return RedirectToAction(nameof(NotTurma));
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotTurma()
        {
            return View();
        }
    }
}