using Acompanhar.Models;
using Microsoft.EntityFrameworkCore;

namespace Acompanhar.Data
{
    public class AcompanharContext : DbContext
    {
        public DbSet<Professor> Professor { get; set; }

        public DbSet<Questionario> Questionario { get; set; }

        public DbSet<Questao> Questao { get; set; }

        public DbSet<Alternativa> Alternativa { get; set; }

        public DbSet<Tarefa> Tarefa { get; set; }
        public DbSet<Administrador> Administrador { get; set; }

        public AcompanharContext(DbContextOptions<AcompanharContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Professor>()
            .HasMany(p => p.Questionarios)
            .WithOne(p => p.Professor)
            .HasForeignKey(p => p.ProfessorId);

            modelBuilder.Entity<Questionario>()
            .HasMany(q => q.Questoes)
            .WithOne(q => q.Questionario)
            .HasForeignKey(q => q.QuestionarioId);

            modelBuilder.Entity<Questionario>()
            .HasMany(q => q.Tarefas)
            .WithOne(t => t.Questionario)
            .HasForeignKey(t => t.QuestionarioId);

            modelBuilder.Entity<Questao>()
            .HasMany(q => q.Alternativas)
            .WithOne(a => a.Questao)
            .HasForeignKey(a => a.QuestaoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
