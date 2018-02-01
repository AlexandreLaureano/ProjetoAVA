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
using Projeto.Alfa12.Models.AccountViewModels;

namespace Projeto.Alfa12.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class ProfessoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfessoresController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Professores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Professores.ToListAsync());
        }

        // GET: Professores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professores
                .SingleOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

      

        // GET: Professores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var professor = await _context.Professores.SingleOrDefaultAsync(m => m.Id == id);
            if (professor == null)
            {
                return NotFound();
            }
            return View(professor);
        }

        // POST: Professores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Autenticado,Nome,Sobrenome,Ativo,DataNascimento,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] Professor professor)
        {
            if (id != professor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(professor);
                    await _context.SaveChangesAsync();

                    var user = _userManager.GetUserAsync(User);
                    LogUsuariosController log = new LogUsuariosController(_context);
                    await log.SetLog("Update Professor : " + professor.FullName, user.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfessorExists(professor.Id))
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
            return View(professor);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AlunoRegister model, string returnUrl = null)
        {


            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new Professor
                {
                    UserName = model.Email,
                    Email = model.Email,
                    DataNascimento = model.Birthday,
                    Ativo = true,
                   
                    Nome = model.Firstname,
                    Sobrenome = model.Lastname,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Professor");


                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
                    //  await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        private bool ProfessorExists(int id)
        {
            return _context.Professores.Any(e => e.Id == id);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
