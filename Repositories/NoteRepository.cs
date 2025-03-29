using BlogNotes.Data;
using BlogNotes.Interfaces;
using BlogNotes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Azure.Core.HttpHeader;

namespace BlogNotes.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly NotesDbContext _context;
        private readonly ILogger<NoteRepository> _logger;

        public NoteRepository(NotesDbContext context, ILogger<NoteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // CREATE NOTE
        public async Task AddAsync(Note note)
        {
            try
            {
                await _context.Notes.AddAsync(note);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository Error: Failed to add note with id {NoteId}", note.Id); throw;
            }
        }

        // DELETE NOTE
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);
                if (note != null)
                {
                    _context.Notes.Remove(note);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository Error: Failed to delete note with id {NoteId}", id);
                throw;
            }

        }

        // GEL ALL NOTES
        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            try
            {
                return await _context.Notes.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Repository Error: Failed to retrieve all notes.");
                throw;
            }
        }

        // GET NOTE BY ID
        public async Task<Note> GetByIdAsync(Guid id)
        {
            try
            {
                var note = await _context.Notes.FindAsync(id);
                return note;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Repository Error: Failed to retrieve note with id {NoteId}", id);
                throw;
            }
        }

        // UPDATE NOTE BY ID
        public async Task UpdateAsync(Note note)
        {
            try
            {
                _context.Notes.Update(note);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex){
                _logger.LogError(ex, "Repository Error: Failed to update note with id {NoteId}", note.Id);
                throw;
            }
        }
    }
}