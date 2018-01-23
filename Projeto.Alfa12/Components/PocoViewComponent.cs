using Projeto.Alfa12.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Components
{
    public class PocoViewComponent
    {
        private readonly ApplicationDbContext _context;

        public PocoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public string Invoke()
        {
            return $"{_context.Alunos.Count()} alunos,"
                + $"{_context.Alunos.Sum(x => x.PontoGeral)} pontos";
        }
    }
}
