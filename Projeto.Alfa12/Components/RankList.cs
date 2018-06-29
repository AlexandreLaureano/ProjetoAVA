using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Components
{
    public class RankList : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public RankList(ApplicationDbContext context)
        {
            _context = context;
        }

        //adicionar argumento para escolher que tipo de lista devo retornar, diaria/mensal/outras
        public async Task<IViewComponentResult> InvokeAsync(int op,bool? showList, int turma)
        {
            if (op == 0)//take 5, lista do Index
            {
                var lista = _context.Pontuacoes;
                var alunos = _context.Alunos;
                foreach (var a in alunos)
                {
                    int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                    a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                }
                return View("RankLista", alunos.OrderByDescending(x => x.PontoParcial).Take(5));
            }
            else if (op == 1)// Lista total
            {
                var lista = _context.Pontuacoes;
                var alunos = _context.Alunos;
                foreach (var a in alunos)
                {
                    int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                    a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                }
                return View("RankLista", alunos.OrderByDescending(x => x.PontoParcial));

            }
            else if(op == 2){// Lista de 7 dias
                var lista = _context.Pontuacoes.Where(x =>/* x.Data <= DateTime.Now &&*/ x.Data >= DateTime.Now.AddDays(-7));
                var alunos = _context.Alunos;
                foreach (var a in alunos)
                {
                    int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                    a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos );
                }
                return View("RankLista", alunos.OrderByDescending(x=>x.PontoParcial));
            }
            else if (op == 3)
            {// Lista do mês
                var lista = _context.Pontuacoes.Where(x =>/* x.Data <= DateTime.Now &&*/ x.Data >= DateTime.Now.AddDays(-30));
                var alunos = _context.Alunos;
                foreach (var a in alunos)
                {
                    int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                    a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                }
                return View("RankLista", alunos.OrderByDescending(x => x.PontoParcial));
            }
            else if(op == 4)// List por turma
            {
                var lista = _context.Pontuacoes.Where(x => x.TurmaId==turma);
                var t = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(y => y.Id == turma);
                var alunos = t.IAluno.Distinct();
                foreach (var a in alunos)
                {
                    int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                    a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                }

                return View("RankLista", alunos.OrderByDescending(x => x.PontoParcial));

            }
            else
            {
                return View(new RankViewModel
                {
                    Nome = _context.Alunos.Count(),
                    Ponto = _context.Alunos.Sum(x => x.PontoGeral)
                });
            }
        }

        public IViewComponentResult InvokePorcent(int op, bool? showList, int turma)
        {

            /*
select AlunoId,count(id) as qtd,sum(Pontos) as pontos,(
select count(id) from modulos where TurmaId=2) as total
  from pontuacoes where (turmaid=2 and Respondido=1)
group by AlunoId

            select de aluno, qnt feita, total ponto, qt total, falta fazer o calculo
 */
            if (op == 1)// Lista total
            {
                
                var lista = _context.Pontuacoes;
                var alunos = _context.Alunos;
                foreach (var a in alunos)
                {
                    int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                    a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                }
                return View("RankLista", alunos.OrderByDescending(x => x.PontoParcial));

            }

            return View(new RankViewModel
            {
                Nome = _context.Alunos.Count(),
                Ponto = _context.Alunos.Sum(x => x.PontoGeral)
            });
        }

       

        /*public IViewComponentResult Invoke() {
        return new HtmlContentViewComponentResult(
        new HtmlString("This is a <h3><i>string</i></h3>"));
        }*/
    }
}
