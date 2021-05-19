using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Acompanhar.Models;

namespace Acompanhar.Data
{
    public class AcompanharContext : DbContext
    {
        public AcompanharContext (DbContextOptions<AcompanharContext> options)
            : base(options)
        {
        }

        public DbSet<Acompanhar.Models.Professor> Professor { get; set; }

        public DbSet<Acompanhar.Models.Questionario> Questionario { get; set; }

        public DbSet<Acompanhar.Models.Questao> Questao { get; set; }

        public DbSet<Acompanhar.Models.Alternativa> Alternativa { get; set; }

        public DbSet<Acompanhar.Models.Tarefa> Tarefa { get; set; }
        public DbSet<Acompanhar.Models.Administrador> Administrador { get; set; }

    }
}
