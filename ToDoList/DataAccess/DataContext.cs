using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext:DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<NoteOwner> NoteOwners { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;
    public DataContext(DbContextOptions<DataContext> options): base(options){}
    
}