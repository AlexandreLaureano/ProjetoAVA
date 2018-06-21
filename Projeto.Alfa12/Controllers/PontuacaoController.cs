using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;
using System;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Controllers
{

    public class PontuacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PontuacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddResposta(Pontuacao p)
        {
           
            _context.Add(p);
            _context.SaveChanges();
        }

        public async Task AddPoint(int id,int pontos)
        {
            var p = await _context.Pontuacoes.SingleOrDefaultAsync(m => m.Id == id);
               // (m => m.ModuloId==pontuacao.ModuloId && m.AlunoId==pontuacao.AlunoId);

            //Pontuacao p = pontuacao;
            p.Pontos = pontos;

            _context.Pontuacoes.Update(p);
            _context.SaveChanges();
            
            LogUsuariosController log = new LogUsuariosController(_context);
            await log.SetLog(p.Pontos+" pontos ganho : Módulo- " + p.ModuloId + " Turma-" +p.TurmaId, p.AlunoId);
            
        }
    }
}