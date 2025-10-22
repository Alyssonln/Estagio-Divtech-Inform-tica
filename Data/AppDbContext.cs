using Microsoft.EntityFrameworkCore;
using FornecedoresApp.Models;

namespace FornecedoresApp.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
  }
}
