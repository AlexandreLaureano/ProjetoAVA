using Microsoft.AspNetCore.Mvc;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;
using System;

namespace Projeto.Alfa12.Controllers
{
    public class PontuacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PontuacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddPoint(int aluno,int? turma,int modulo,int ponto = 1)
        {
            Pontuacao p = new Pontuacao
            {
                AlunoId = aluno,
                TurmaId = turma,
                ModuloId = modulo,
                Pontos = ponto,
                Data = DateTime.Now
            };
            _context.Pontuacoes.Add(p);
            _context.SaveChanges();
        }
    }
}