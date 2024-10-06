using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjetoApresentacao.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProjetoApresentacao.Data
{
    public class AppDbContext : DbContext, IDisposable
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            Database.Migrate();

        }


        public AppDbContext()
        {

            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=.;Database=ApresentacaoDB;User ID=sa;Password=sql2022.;Trusted_Connection=False;Encrypt=False;");
        }

        public void Dispose()
        {
            // Fecha a conexão com o banco de dados
            Database.CloseConnection();
        }
        
        
        
        public DbSet<Produto> Produto { get; set; }

    }
}
