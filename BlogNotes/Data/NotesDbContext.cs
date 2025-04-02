using BlogNotes.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogNotes.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional settings
            modelBuilder.Entity<Note>().HasKey(n => n.Id);
        }
    }
}
