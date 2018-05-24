using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Projeto.Alfa12.Data;
using Projeto.Alfa12.Models;
using Projeto.Alfa12.Models.CreateViewModels;

namespace Projeto.Alfa12.Controllers
{
    //[Authorize(Roles = "Professor")]
    public class ModulosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private static readonly HttpClient client = new HttpClient();

        public ModulosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Modulos
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User);
            var professor = (Professor)await user;

            var applicationDbContext = _context.Modulos.Include(m => m.Turma).Where(x => x.Turma.ProfessorId == professor.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Modulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var user = _userManager.GetUserAsync(User);
            var aluno = await user;
            var modulo = await _context.Modulos.Include("Pontuacao.Modulo").SingleOrDefaultAsync(m => m.Id == id);
            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == modulo.TurmaId);
            
            if (turma.IAluno.Contains(aluno))
            {
               /* if (id == null)
                {
                    return NotFound();
                }


                if (modulo == null)
                {
                    return NotFound();
                }*/

                return View(modulo);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> RespAluno(int? id) {
            var user = _userManager.GetUserAsync(User);
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

                return View("Respostas/RespAluno",modulo);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespAluno(int id, [Bind("Id,Resposta")] Modulo modulo)
        {
            var user = (Aluno)await _userManager.GetUserAsync(User);
            if (id != modulo.Id)
            {
                return NotFound();
            }

            var mod = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);

            mod.Resposta = modulo.Resposta;
            Pontuacao p = new Pontuacao
            {
                AlunoId = user.Id,
                Data = DateTime.Now,
                ModuloId = mod.Id,
                Respondido = true,
                Resposta = mod.Resposta,
                TurmaId = mod.TurmaId

            };
            PontuacaoController pc = new PontuacaoController(_context);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mod);
                    pc.AddResposta(p);
                    await _context.SaveChangesAsync();

                    LogUsuariosController log = new LogUsuariosController(_context);
                    await log.SetLog("Resposta inserida : " + mod.Nome, user.Id);

                    TempData["alert"] = $"{modulo.Nome} foi respondido";
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
            return View();
        }


            public async Task<IActionResult> RespProfessor(int? id)
        {
            var user = _userManager.GetUserAsync(User);
            var professor = await user;
            var modulo = await _context.Modulos.Include(m=>m.Pontuacao).SingleOrDefaultAsync(m => m.Id == id);
            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == modulo.TurmaId);

            if (turma.Professor.Equals(professor))
            {
                if (id == null)
                {
                    return NotFound();
                }


                if (modulo == null)
                {
                    return NotFound();
                }
                
                return View("Respostas/RespProfessor",modulo);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespProfessor(int id, [Bind("Id,Pontos,idpontuacao")] Modulo modulo)
        {
            PontuacaoController pc = new PontuacaoController(_context);
            await pc.AddPoint(modulo.idpontuacao, modulo.Pontos);

            var user = (Professor)await _userManager.GetUserAsync(User);
            LogUsuariosController log = new LogUsuariosController(_context);
            await log.SetLog("Nota inserida : " + modulo.Nome, user.Id);

            TempData["alert"] = $"{modulo.Nome} foi atribuido nota";

            var mod = await _context.Modulos.Include(m => m.Pontuacao).SingleOrDefaultAsync(m => m.Id == id);

            return View("Respostas/RespProfessor", mod);
        }



            // GET: Modulos/Create
            public async Task<IActionResult> Create()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create");
        }
        public async Task<IActionResult> Create2()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create2");
        }
        public async Task<IActionResult> Create3()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create3");
        }
        public async Task<IActionResult> Create4()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create4");
        }
        public async Task<IActionResult> Create5()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create5");
        }

   

        // POST: Modulos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuloViewModel modulo)
        {
           
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            Modulo mod = new Modulo
            {
                Nome = modulo.Nome,
                Descricao = modulo.Descricao,
                TurmaId = modulo.TurmaId,
                Url = modulo.Url,
                Resposta = modulo.Resposta,
                Texto = modulo.Texto,
                Tipo = (TipoMod)modulo.Tipo,
                MaxPonto = modulo.MaxPonto
            };

            if (ModelState.IsValid)
            {
                if (modulo.Arquivo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await modulo.Arquivo.CopyToAsync(memoryStream);
                        mod.Arquivo = memoryStream.ToArray();
                    }
                }
                _context.Add(mod);
                await _context.SaveChangesAsync();

                LogUsuariosController log = new LogUsuariosController(_context);
                await log.SetLog("Create Modulo :" + mod.Nome, user.Id);
                TempData["alert"] = $"{mod.Nome} foi criado";
                return RedirectToAction(nameof(Index));
            }
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Id", modulo.TurmaId);
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
            var user = (Professor) await _userManager.GetUserAsync(User);
            
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

                    LogUsuariosController log = new LogUsuariosController(_context);
                    await log.SetLog("Update Modulo : " + modulo.Nome, user.Id);

                    TempData["alert"] = $"{modulo.Nome} foi criado";
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

            var user = await _userManager.GetUserAsync(User);
            LogUsuariosController log = new LogUsuariosController(_context);
            await log.SetLog("Delete Modulo : " + modulo.Nome +" " + modulo.Id, user.Id);

            TempData["alert"] = $"{modulo.Nome} foi deletado";

            return RedirectToAction(nameof(Index));
        }

        private bool ModuloExists(int id)
        {
            return _context.Modulos.Any(e => e.Id == id);
        }

        //[EnableCors("AllowAll")]
                public IActionResult Home(int id)
        {
            return View(_context.Modulos.SingleOrDefault(x=>x.Id==id));
        }

        [HttpPost]
        [Authorize(Roles = "Aluno")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPonto(int id)
        {
            var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
            int x;
            //url da pagina + algo

            string myJson = "  { 'Acerto' : 's','Ponto': '3', 'y' = '1', 'x' = '2' }";
            Pontos t = new Pontos { Acerto = true, ponto = 3, y = "1", x = "2" };

            //string myJson = "{'Username': 'myusername','Password':'pass'}";
            using (var client = new HttpClient())
            {
                // var response = await client.PostAsync(
                //   "http://localhost:64466/api/values",
                //  new StringContent(myJson, Encoding.UTF8, "application/json"));
                HttpRequestMessage request = 
                    new HttpRequestMessage(HttpMethod.Post, "http://localhost:64466/api/values");

                string json = JsonConvert.SerializeObject(t);

                request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpClient http = new HttpClient();
                HttpResponseMessage response = await http.SendAsync(request);

                var responseString = await response.Content.ReadAsStringAsync();
                var post = JsonConvert.DeserializeObject<Pontos>(responseString.ToString());
                x = post.ponto;
            }




            /*var requisicaoWeb = WebRequest.CreateHttp(" http://localhost:64466/api/values/2");
            requisicaoWeb.Method = "GET";
           
            using (var resposta = requisicaoWeb.GetResponse())
            {
                var streamDados = resposta.GetResponseStream();
                StreamReader reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();

                var post = JsonConvert.DeserializeObject<Teste>(objResponse.ToString());
                x = post.Ponto;
                streamDados.Close();
                resposta.Close();
            }*/
            

            var user = _userManager.GetUserAsync(User);
            var aluno = (Aluno)await user;

            PontuacaoController p = new PontuacaoController(_context);
          //  await p.AddPoint(aluno.Id, modulo.TurmaId, modulo.Id, x);
            
            

            return RedirectToAction(nameof(Index));
        }

     
    }

    public class Pontos{
        public bool Acerto;
       public int ponto;
       public string x, y;
        }
}
