using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.Data;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Lahko dodaš konfiguracijo tukaj, če je potrebno
        base.OnModelCreating(modelBuilder);
    }
}