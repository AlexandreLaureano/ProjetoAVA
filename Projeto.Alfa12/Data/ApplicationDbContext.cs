using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projeto.Alfa12.Models;

namespace Projeto.Alfa12.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Coordenador> Coordenadores { get; set; }
        public DbSet<AlunoConquista> AlunoConquistas { get; set; }
        public DbSet<AlunoGamification> AlunoGamifications { get; set; }
        public DbSet<AlunoTurma> AlunoTurmas { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Conquista> Conquistas { get; set; }
        public DbSet<LogUsuario> Logs { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Pontuacao> Pontuacoes { get; set; }
        public DbSet<Turma> Turmas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Turma>()
                .HasOne(t => t.Professor)
                .WithMany(b => b.Turmas)
                .OnDelete(DeleteBehavior.Restrict);
            //.HasForeignKey<Professor>(b => b.ProfessorId);

            builder.Entity<Mensagem>()
                .HasOne(t => t.Origem)
                .WithMany(s => s.Origem)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Mensagem>()
               .HasOne(t => t.Destino)
               .WithMany(s => s.Destino)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AlunoConquista>()
                .HasKey(a => new { a.AlunoId, a.ConquistaId });

           

            builder.Entity<AlunoGamification>()
                .HasKey(a => a.AlunoId);

            //Mapeamento aluno/turma
            builder.Entity<AlunoTurma>()
                .HasOne(u => u.Aluno)
                .WithMany("Turmas")
                .HasForeignKey(u => u.AlunoId);

            builder.Entity<AlunoTurma>()
                .HasOne(t => t.Turma)
                .WithMany("Alunos")
                .HasForeignKey(t => t.TurmaId);


            builder.Entity<LogUsuario>()
               .HasOne(x => x.Usuario)
               .WithMany(y => y.Logs)
               .HasForeignKey(l => l.UsuarioId);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(i => {
                i.ToTable("Users");
                i.HasKey(x => x.Id);
            });
            builder.Entity<ApplicationRole>(i => {
                i.ToTable("Role");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserRole<int>>(i => {
                i.ToTable("UserRole");
                i.HasKey(x => new { x.RoleId, x.UserId });
            });
            builder.Entity<IdentityUserLogin<int>>(i => {
                i.ToTable("UserLogin");
                i.HasKey(x => new { x.ProviderKey, x.LoginProvider });
            });
            builder.Entity<IdentityRoleClaim<int>>(i => {
                i.ToTable("RoleClaims");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserClaim<int>>(i => {
                i.ToTable("UserClaims");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserToken<int>>(i =>
            {
                i.ToTable("UserTokens");
                i.HasKey(x => x.UserId);
            });
        }
    }
}
