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