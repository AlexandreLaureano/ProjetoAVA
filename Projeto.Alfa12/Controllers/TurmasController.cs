using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;

namespace Projeto.Alfa12.Controllers
{
    //[Authorize]
    public class TurmasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TurmasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        // GET: Turmas
        [AllowAnonymous]
        public async Task<IActionResult> Index(string SearchString = "")
        {
            //var turma = from t in _context.Turmas select t; colocar join
            var turma = _context.Turmas.Include(t => t.Professor);

            if (!String.IsNullOrEmpty(SearchString))
            {
                turma = turma.Where(s => s.Nome.Contains(SearchString)).Include(t =>t.Professor);
            }

            return View(await turma.ToListAsync());
        }

        //[Authorize(Roles = "Professor")]
        public async Task<IActionResult> IndexProfessor()
        {
            var user = (Professor) await _userManager.GetUserAsync(User); 
            var turmas = _context.Turmas.Include(t => t.Professor).Where(p => p.ProfessorId.Equals(user.Id));
            
            return View(await turmas.ToListAsync());
        }

        //[Authorize(Roles = "Aluno")]
        public async Task<IActionResult> IndexAluno()
        {
            var user = (Aluno)await _userManager.GetUserAsync(User);
            List<Turma> Lturma = new List<Turma>();
            var turmas = _context.Turmas.Include("Alunos.Aluno");
      
            foreach (Turma t in turmas)
            {
                if (t.IAluno.Contains(user))
                {
                    Lturma.Add(t);
                }
            }
            return View(Lturma);
        }

        public async Task<IActionResult> IndexAdmin(string SearchString = "")
        {
            //var turma = from t in _context.Turmas select t; colocar join
            var turma = _context.Turmas.Include(t => t.Professor);

            if (!String.IsNullOrEmpty(SearchString))
            {
                turma = turma.Where(s => s.Nome.Contains(SearchString)).Include(t => t.Professor);
            }

            return View(await turma.ToListAsync());
        }

        // GET: Turmas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas
                .Include(t => t.Professor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        // GET: Turmas/Create
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Turmas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,AreaConhecimento,ChaveAcesso,MaxPonto")] Turma turma)
        {
            var user = _userManager.GetUserAsync(User);
            turma.Professor = (Professor)await user;
            DateTime hoje = DateTime.Now;
            turma.DataCriacao = hoje;
            turma.MaxPonto = 100;
            
           
            if (ModelState.IsValid)
            {
                _context.Add(turma);
                await _context.SaveChangesAsync();

                LogUsuariosController log = new LogUsuariosController(_context);
                await log.SetLog("Create Turma : " + turma.Nome, turma.Professor.Id);


                return RedirectToAction(nameof(Index));
            }
            return View(turma);
        }

        // GET: Turmas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas.SingleOrDefaultAsync(m => m.Id == id);
            if (turma == null)
            {
                return NotFound();
            }
            //                          **receber nome e sobrenome mostrar no form **
            ViewData["ProfessorId"] = new SelectList(_context.Professores, "Id", "Nome", turma.ProfessorId);
            return View(turma);
        }

        // POST: Turmas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,AreaConhecimento,ChaveAcesso,ProfessorId")] Turma turma)
        {
            if (id != turma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turma);
                    await _context.SaveChangesAsync();

                    var user = (ApplicationUser) await _userManager.GetUserAsync(User);

                    LogUsuariosController log = new LogUsuariosController(_context);
                    await log.SetLog("Edit Turma => Id:" +turma.Id + ", Nome:" + turma.Nome, user.Id);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurmaExists(turma.Id))
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
            ViewData["ProfessorId"] = new SelectList(_context.Professores, "Id", "Nome", turma.ProfessorId);
            return View(turma);
        }

        // GET: Turmas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas
                .Include(t => t.Professor)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

        // POST: Turmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turma = await _context.Turmas.SingleOrDefaultAsync(m => m.Id == id);
            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();

            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            LogUsuariosController log = new LogUsuariosController(_context);
            await log.SetLog("Delete Turma => Id:" + turma.Id + ", Nome:" + turma.Nome, user.Id);

            return RedirectToAction(nameof(Index));
        }

        private bool TurmaExists(int id)
        {
            return _context.Turmas.Any(e => e.Id == id);
        }


        public async Task<ViewResult> AddAluno(int id)
        {
            List<Aluno> members = new List<Aluno>();
            List<Aluno> nonMembers = new List<Aluno>();

            /* var posts = _context.Turmas.Include("UserTurma.User").Include(x=> x.User).ToList();
            var postss = _context.Turmas.Include(e => e.UserTurma).ToList(); // Won't work*/

            //var turma = await _context.Turmas.Include("AlunoTurma.Aluno").Include(x => x.IAluno).SingleOrDefaultAsync(i => i.Id == id);
            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == id);
            //var turma = await _context.Turmas.Include(x => x.Alunos).Include(y => y.Alunos).SingleOrDefaultAsync(i => i.Id == id);
            turma.Id = id;
            var alunos = _context.Alunos.ToList();

            alunos.Count();
            foreach (Aluno user in alunos)
            {

                var list = turma.IAluno.Contains(user)
                 ? members : nonMembers;
                list.Add(user);

            }

            return View(new TurmaEditModel
            {
                Turma = turma,
                Members = members,
                NonMembers = nonMembers
            });

        }

        [HttpPost]
        public async Task<IActionResult> AddAluno(int id, TurmaModificationModel model)
        {
            if (id != model.TurmaId)
            {
                return NotFound();
            }
            //var turma = await _context.Turmas.Include("AlunoTurma.Aluno").Include(x => x.IAluno).SingleOrDefaultAsync(i => i.Id == id);

           // var turma = await _context.Turmas.Include(x => x.Alunos).Include(y => y.Alunos).SingleOrDefaultAsync(i => i.Id == id);

            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == id);

            var context = _context.AlunoTurmas;
            
            if (ModelState.IsValid)
            {
                try
                {
                    //var turma = turmas.SingleOrDefault(x => x.Id == model.TurmaId);

                    foreach (int userId in model.IdsToAdd ?? new int[] { })
                    {
                        Aluno user = await _context.Alunos.SingleOrDefaultAsync(y => y.Id == userId);
                        if (user != null)
                        {
                            AlunoTurma at = new AlunoTurma {
                                AlunoId = user.Id,
                                TurmaId = turma.Id,
                               // Ativo = true,
                                DataIngressao = DateTime.Now        
                        };
                            _context.AlunoTurmas.Update(at);
                            //turma.UserTurma.Add(new AlunoTurma { TurmaId = turma.Id, AlunoId = user.Id });
                            /// turma.ITurma.Add(user);
                        }
                    }
                    //turmas[turmas. IndexOf(turma)] = turma;
                    //_context.Update(turma);
                    await _context.SaveChangesAsync();
                    foreach (int userId in model.IdsToDelete ?? new int[] { })
                    {
                        Aluno user = await _context.Alunos.SingleOrDefaultAsync(y => y.Id == userId);
                        if (user != null)
                        {

                            AlunoTurma ut = new AlunoTurma();
                            ut = _context.AlunoTurmas.SingleOrDefault(y =>  y.AlunoId == userId && y.TurmaId == turma.Id );
                            _context.AlunoTurmas.Remove(ut);
                            //turma.Aluno.Remove(ut);


                        }
                    }
                    //turmas[turmas.IndexOf(turma)] = turma;
                    //_context.Update(turma);
                    await _context.SaveChangesAsync();

                    var usuario = (ApplicationUser)await _userManager.GetUserAsync(User);
                    LogUsuariosController log = new LogUsuariosController(_context);
                    await log.SetLog("Update Alunos da Turma => Id:" + turma.Id + ", Nome:" + turma.Nome, usuario.Id);
                }
                catch (DbUpdateConcurrencyException)
                {

                    return NotFound();

                }
                return RedirectToAction(nameof(Index));
            }
            return await AddAluno(model.TurmaId);
        }

        public IActionResult EntrarTurma()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EntrarTurma(int id, [Bind("Id,ChaveAcesso")] Turma chave)
        {
            var turma = await _context.Turmas.SingleOrDefaultAsync(m => m.Id == id);
            var user = _userManager.GetUserAsync(User);
            Aluno aluno = (Aluno)await user;
            //var senha = chave.ChaveAcesso.Equals(null)  ? "" : chave.ChaveAcesso;
            if (chave.ChaveAcesso == null)
            {
                AlunoTurma at = new AlunoTurma
                {
                    AlunoId = aluno.Id,
                    TurmaId = turma.Id,
                    // Ativo = true,
                    DataIngressao = DateTime.Now
                };
                _context.AlunoTurmas.Update(at);
            }
            else
            {
                if (chave.ChaveAcesso.Equals(turma.ChaveAcesso))
                {

                    AlunoTurma at = new AlunoTurma
                    {
                        AlunoId = aluno.Id,
                        TurmaId = turma.Id,
                        // Ativo = true,
                        DataIngressao = DateTime.Now
                    };
                    _context.AlunoTurmas.Update(at);
                }
                else
                {
                    return NotFound();
                }
            }
           
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Home(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var turma = await _context.Turmas
                .Include(t => t.Professor)
                .Include(m => m.Modulos)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (turma == null)
            {
                return NotFound();
            }

            return View(turma);
        }

    }
}
