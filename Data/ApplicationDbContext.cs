using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace app_parcial.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<app_parcial.Models.Transaccion> DataTransaccion {get; set; }
    public DbSet<app_parcial.Models.HistorialConversion> DataHistorialConversion {get; set; }
}
