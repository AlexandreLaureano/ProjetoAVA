﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;

namespace Projeto.Alfa12.Controllers
{
    //[Authorize(Roles = "Professor")]
    public class ModulosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ModulosController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Modulos
        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User);
            var professor = (Professor)await user;

            var applicationDbContext = _context.Modulos.Include(m => m.Turma).Where(x=>x.Turma.ProfessorId==professor.Id);
            
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Modulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var user = _userManager.GetUserAsync(User);
            var aluno = (Aluno)await user;
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

                return View(modulo);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Modulos/Create
        public IActionResult Create()
        {
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Nome");
            return View();
        }

        // POST: Modulos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Url,TurmaId")] Modulo modulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modulo);
                await _context.SaveChangesAsync();
                var user = (ApplicationUser)await _userManager.GetUserAsync(User);
                LogUsuariosController log = new LogUsuariosController(_context);
                await log.SetLog("Create Modulo :" + modulo.Nome, user.Id);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Id", modulo.TurmaId);
            return View(modulo);
        }

        // GET: Modulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = _userManager.GetUserAsync(User);
            var professor = (Professor)await user;
            var modulo = await _context.Modulos.FirstOrDefaultAsync(x => x.Id == id);
            var turma = await _context.Turmas.FirstOrDefaultAsync(x => x.Id == modulo.TurmaId);
            if (turma.ProfessorId == professor.Id)
            {

                if (id == null)
                {
                    return NotFound();
                }
                
                if (modulo == null)
                {
                    return NotFound();
                }
                ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Id", modulo.TurmaId);
                return View(modulo);
            }
            else
            {
               
                return RedirectToRoute(nameof(Index));
            }
        }

        // POST: Modulos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Url,TurmaId")] Modulo modulo)
        {
            if (id != modulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuloExists(modulo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TurmaId"] = new SelectList(_context.Turmas, "Id", "Id", modulo.TurmaId);
            return View(modulo);
        }

        // GET: Modulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modulo = await _context.Modulos
                .Include(m => m.Turma)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (modulo == null)
            {
                return NotFound();
            }

            return View(modulo);
        }

        // POST: Modulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
            _context.Modulos.Remove(modulo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuloExists(int id)
        {
            return _context.Modulos.Any(e => e.Id == id);
        }
    }
}
