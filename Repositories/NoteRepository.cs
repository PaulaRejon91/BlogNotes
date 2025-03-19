using BlogNotes.Data;
using BlogNotes.Interfaces;
using BlogNotes.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogNotes.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly NotesDbContext _context;

        public NoteRepository(NotesDbContext context)
        {
            _context = context;
        }

        //CREATE NOTE
        public async Task AddAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }

        //DELETE NOTE
        public async Task DeleteAsync(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        //GEL ALL NOTES
        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        //GET NOTE BY ID
        public async Task<Note> GetByIdAsync(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            return note;
        }

        //UPDATE NOTE BY ID
        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }
    }
}