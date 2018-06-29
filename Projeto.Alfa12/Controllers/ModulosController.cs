using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        public async Task<IActionResult> Index()
        {
            var user = _userManager.GetUserAsync(User);
            if (User.IsInRole("Professor"))
            {
                var professor = (Professor)await user;

                var applicationDbContext = _context.Modulos.Include(m => m.Turma).Where(x => x.Turma.ProfessorId == professor.Id);

                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                return View(await _context.Modulos.Include(m => m.Turma).ToListAsync());
            }
        }

        // GET: Modulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            //var user = _userManager.GetUserAsync(User);
           // var aluno = await user;
            var modulo = await _context.Modulos.Include("Pontuacao.Modulo").SingleOrDefaultAsync(m => m.Id == id);
            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == modulo.TurmaId);

            return View(modulo);
            /*if (turma.IAluno.Contains(aluno))
            {
               /* if (id == null)
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
            }*/
        }

        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> RespAluno(int? id) {
            var user = _userManager.GetUserAsync(User);
            var aluno = await user;
            var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == modulo.TurmaId);
            var pontuacao = await _context.Pontuacoes.Where(x => x.ModuloId == id && x.AlunoId == aluno.Id).SingleOrDefaultAsync();

            ModuloPontuacaoViewModel MP = new ModuloPontuacaoViewModel
            {
                modulo = modulo,
                pontuacao = pontuacao
            };

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

                return View("Respostas/RespAluno",MP);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Aluno")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespAluno(int id, ModuloPontuacaoViewModel mp)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (id != mp.modulo.Id)
                        {
                            return NotFound();
                        }

                    var user = (Aluno)await _userManager.GetUserAsync(User);
                    var mod = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);

                    //mod.Resposta = modulo.Resposta;
                    mod.Respondido = true;
                    //Os pontos, o professor adiciona
                    Pontuacao p = new Pontuacao
                    {
                        AlunoId = user.Id,
                        Data = DateTime.Now,
                        ModuloId = mod.Id,
                        Respondido = true,
                        Resposta = mp.pontuacao.Resposta,
                        TurmaId = mod.TurmaId
                    };

                    PontuacaoController pc = new PontuacaoController(_context);
                    LogUsuariosController log = new LogUsuariosController(_context);

                    _context.Update(mod);
                    pc.AddResposta(p);
                    await log.SetLog("Resposta inserida : " + mod.Nome, user.Id);

                    await _context.SaveChangesAsync();

                    TempData["alert"] = $"{mp.modulo.Nome} foi respondido";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuloExists(mp.modulo.Id))
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

        [Authorize(Roles = "Aluno")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespAlunoOnline(int id, [Bind("Id,Resposta")] Modulo modulo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id != modulo.Id)
                        {
                            return NotFound();
                        }

                    var user = _userManager.GetUserAsync(User);
                    var aluno = (Aluno)await user;
                    var mod = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
                    int x;

            //string myJson = "  { 'Acerto' : 's','Ponto': '3', 'y' = '1', 'x' = '2' }";
            //classe de dados de envio
                    RequisicaoResposta RR = new RequisicaoResposta
                    {
                        aluno = aluno.Id,
                        modulo = mod.Id,
                        resposta = modulo.Resposta
                    };
                    RequisicaoResposta resulthttp;

            
                    using (var client = new HttpClient())
                    {
                        // var response = await client.PostAsync(
                        //   "http://localhost:64466/api/values",
                        //  new StringContent(myJson, Encoding.UTF8, "application/json"));
                        HttpRequestMessage request =
                          //  new HttpRequestMessage(HttpMethod.Post, "http://localhost:64466/api/values");
                        new HttpRequestMessage(HttpMethod.Post, mod.Url);

                        string json = JsonConvert.SerializeObject(RR);
                        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        HttpClient http = new HttpClient();
                        HttpResponseMessage response = await http.SendAsync(request);

                        var responseString = await response.Content.ReadAsStringAsync();
                        var post = JsonConvert.DeserializeObject<RequisicaoResposta>(responseString.ToString());
                       resulthttp = post;
                        x = post.ponto;
                    }

                    PontuacaoController pc = new PontuacaoController(_context);
                    LogUsuariosController log = new LogUsuariosController(_context);

                    //tratar porcentagem de acerto
                    if (resulthttp.Acerto.Equals(true))
                    {
                        mod.Resposta = resulthttp.resposta;
                        mod.Pontos = resulthttp.ponto;
                        mod.Respondido = true;
                    }
                    else
                    {
                        mod.Pontos = 0;
                        mod.Respondido = true;
                    }

                    Pontuacao p = new Pontuacao
                    {
                        AlunoId = aluno.Id,
                        Data = DateTime.Now,
                        ModuloId = mod.Id,
                        Respondido = true,
                        Resposta = mod.Resposta,
                        TurmaId = mod.TurmaId,
                        Pontos = resulthttp.ponto
                    };
          
           
                    _context.Update(mod);
                    pc.AddResposta(p);
                    await log.SetLog("Resposta inserida : " + mod.Nome, user.Id);
                    await _context.SaveChangesAsync();

                    TempData["alert"] = $"{mod.Nome} foi respondido";
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

        [Authorize(Roles = "Administrador,Professor")]
        public async Task<IActionResult> RespProfessor(int? id)
        {
            var user = _userManager.GetUserAsync(User);
            var professor = await user;
            var modulo = await _context.Modulos.Include(m=>m.Pontuacao).SingleOrDefaultAsync(m => m.Id == id);
            var turma = await _context.Turmas.Include("Alunos.Aluno").SingleOrDefaultAsync(i => i.Id == modulo.TurmaId);

            var alunos = _context.Alunos.Include(p => p.Pontuacao.Where(m => m.ModuloId == id).SingleOrDefault());

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

        [Authorize(Roles = "Administrador,Professor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespProfessor(int id, [Bind("Id,Pontos,idpontuacao")] Modulo modulo)
        {
            var user = (Professor)await _userManager.GetUserAsync(User);
            PontuacaoController pc = new PontuacaoController(_context);
            LogUsuariosController log = new LogUsuariosController(_context);

            await pc.AddPoint(modulo.idpontuacao, modulo.Pontos);
            await log.SetLog("Nota inserida : " + modulo.Nome, user.Id);

            TempData["alert"] = $"{modulo.Nome} foi atribuido nota";

            var mod = await _context.Modulos.Include(m => m.Pontuacao).SingleOrDefaultAsync(m => m.Id == id);
            return View("Respostas/RespProfessor", mod);
        }



        // GET: Modulos/Create
        [Authorize(Roles = "Administrador,Professor")]
        public async Task<IActionResult> Create()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create");
        }

        [Authorize(Roles = "Administrador,Professor")]
        public async Task<IActionResult> Create2()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create2");
        }

        [Authorize(Roles = "Administrador,Professor")]
        public async Task<IActionResult> Create3()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create3");
        }

        [Authorize(Roles = "Administrador,Professor")]
        public async Task<IActionResult> Create4()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create4");
        }

        [Authorize(Roles = "Administrador,Professor")]
        public async Task<IActionResult> Create5()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Nome");
            return View("Creates/Create5");
        }



        // POST: Modulos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador,Professor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuloViewModel modulo)
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            { 
                Modulo mod = new Modulo
                {
                    Nome = modulo.Nome,
                    Descricao = modulo.Descricao,
                    TurmaId = modulo.TurmaId,
                    Url = modulo.Url,
                    Resposta = modulo.Resposta,
                    Texto = modulo.Texto,
                    Tipo = (TipoMod)modulo.Tipo,
                    MaxPonto = modulo.MaxPonto     ,
                    Respondido = false,
                    DataMax = modulo.DataMax
                    
                };

               
                var turma = await _context.Turmas.FirstOrDefaultAsync(x => x.Id == modulo.TurmaId);

                var pontos = _context.Turmas.Include(x => x.Modulos).Where(t => t.Id == modulo.TurmaId).Sum(x=>x.Modulos.Sum(t=>t.MaxPonto));
                if (pontos + modulo.MaxPonto > turma.MaxPonto)
                {
                    TempData["alert"] = $"Pontuação máxima ultrapassada";
                    return RedirectToAction(nameof(Index));
                }
               
                if (modulo.Arquivo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await modulo.Arquivo.CopyToAsync(memoryStream);
                        mod.Arquivo = memoryStream.ToArray();
                    }
                }

                turma.PontoAtual += modulo.MaxPonto;
                _context.Update(turma);
                _context.Add(mod);
                await _context.SaveChangesAsync();

                LogUsuariosController log = new LogUsuariosController(_context);
                await log.SetLog("Create Modulo :" + mod.Nome, user.Id);
                TempData["alert"] = $"{mod.Nome} foi criado";

                return RedirectToPage("/Turmas/Home/",mod.TurmaId);
            }
            ViewData["TurmaId"] = new SelectList(_context.Turmas.Where(x => x.ProfessorId == user.Id), "Id", "Id", modulo.TurmaId);
            return View(modulo);
        }

        // GET: Modulos/Edit/5
        [Authorize(Roles = "Administrador,Professor")]
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
        [Authorize(Roles = "Administrador,Professor")]
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

                    TempData["alert"] = $"{modulo.Nome} foi alterado";
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
        [Authorize(Roles = "Administrador,Professor")]
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
        [Authorize(Roles = "Administrador,Professor")]
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

        public async Task<ActionResult> Visibilidade(int id,int turma)
        {
            var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
            if (modulo.Visivel == true)
            {
                modulo.Visivel = false;
            }
            else
            {
                modulo.Visivel = true;
            }
            _context.Update(modulo);
            await _context.SaveChangesAsync();



            return RedirectToAction("Home", "Turmas", turma);
            //return  RedirectToAction("Home","Turmas",  ViewData["Turma"]);

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

        //Não utilizado
        [HttpPost]
        [Authorize(Roles = "Aluno")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPonto(int id)
        {
            var modulo = await _context.Modulos.SingleOrDefaultAsync(m => m.Id == id);
            int x;
            
            string myJson = "  { 'Acerto' : 's','Ponto': '3', 'y' = '1', 'x' = '2' }";
            RequisicaoResposta t = new RequisicaoResposta { Acerto = true, ponto = 3};

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
                var post = JsonConvert.DeserializeObject<RequisicaoResposta>(responseString.ToString());
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

    public class RequisicaoResposta{
        public bool Acerto;
       public int ponto;
       public string resposta;
        public int aluno;
        public int modulo;
        }
}
