using Microsoft.EntityFrameworkCore;
using PotatoProject2.Models;

namespace PotatoProject2.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Potato> Potatoes => Set<Potato>();
    public DbSet<User> Users => Set<User>();
}
