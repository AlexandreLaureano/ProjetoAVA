using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;

namespace Projeto.Alfa12.Controllers
{
    public class LogUsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int PageSize = 10;

        public LogUsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var log = _context.Logs.Include(x => x.Usuario).ToList();

            return View(_context.Logs.ToList());
        }

        public ViewResult List(string category, int productPage = 1)
        => View(new LogListViewModel
        {
            Logs = _context.Logs
            .Where(p => category == null || p.Acao.Contains(category))
            .OrderByDescending(p => p.Data)
            .Skip((productPage - 1) * PageSize)
            .Take(PageSize)
            .Include(x => x.Usuario),
            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,        
                TotalItems = category == null ?
                _context.Logs.Count() :
                _context.Logs.Where(e =>
                e.Acao.Contains(category)).Count()
            },
            CurrentCategory = category
        });

        public async Task SetLog(string action, int IdUser)
        {
            LogUsuario log = new LogUsuario
            {
                Acao = action,
                Data = DateTime.Now,
                UsuarioId = IdUser
            };

            _context.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}