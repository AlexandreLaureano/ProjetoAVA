using Microsoft.AspNetCore.Mvc;
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
        public IViewComponentResult Invoke(bool showList)
        {
            if (showList)
            {
                return View("RankLista", _context.Alunos.OrderBy(x => x.PontoGeral));
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
