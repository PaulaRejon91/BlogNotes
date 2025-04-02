using BlogNotes.Models;

namespace BlogNotes.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDto>> GetAllNotesAsync();
        Task<NoteDto> GetNoteByIdAsync(Guid id);
        Task CreateNoteAsync(NoteDto note);
        Task UpdateNoteAsync(NoteDto note);
        Task DeleteNoteAsync(Guid id);
    }
}
