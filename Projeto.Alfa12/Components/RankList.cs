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
        public IViewComponentResult Invoke(int op,bool? showList,int? turma)
        {
            if (op == 0)
            {
                return View("RankLista", _context.Alunos.OrderByDescending(x => x.PontoGeral).Take(5));
            }
            else if(op == 1){
                var lista = _context.Pontuacoes.Where(x => x.Data <= DateTime.Now);
                var alunos = _context.Alunos;
                foreach (var a in alunos)
                {
                    int ponto = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                    a.PontoParcial = lista.Where(x => x.AlunoId == a.Id).Sum(x => x.Pontos);
                }
                return View("RankLista", alunos.OrderByDescending(x=>x.PontoParcial));
            }
            else if(op == 2)
            {
                var lista = _context.Pontuacoes.Where(x => x.TurmaId==turma);
                var t = _context.Turmas.Include("Alunos.Aluno").SingleOrDefault(y => y.Id == turma);
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
/*
        public IViewComponentResult Invoke()
        {
            return View(new RankViewModel
            {
                Nome = _context.Alunos.Count(),
                Ponto = _context.Alunos.Sum(x => x.PontoGeral)
            });
        }
        */
        /*public IViewComponentResult Invoke() {
        return new HtmlContentViewComponentResult(
        new HtmlString("This is a <h3><i>string</i></h3>"));
        }*/
    }
}
