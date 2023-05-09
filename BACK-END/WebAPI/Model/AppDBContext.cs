using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Model
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<FornecedorEmpresa> FornecedorEmpresa { get; set; }


        
    }
}
