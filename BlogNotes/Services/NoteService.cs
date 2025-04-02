using BlogNotes.Interfaces;
using BlogNotes.Mappers.BlogNotes.Mappers;
using BlogNotes.Models;

namespace BlogNotes.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repository;
        private readonly ILogger<NoteService> _logger;

        public NoteService (INoteRepository repository, ILogger<NoteService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // CREATE NOTE
        public async Task CreateNoteAsync(NoteDto noteDto)
        {
            try
            {
                var note = NoteMapper.ToEntity(noteDto);
                await _repository.AddAsync(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service Error: Failed to create note with id {NoteId}", noteDto.Id);
                throw;
            }
        }

        // DELETE NOTE
        public async Task DeleteNoteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service Error: Failed to delete note with id {NoteId}", id);
                throw;
            }
        }

        // GET ALL NOTES
        public async Task<IEnumerable<NoteDto>> GetAllNotesAsync()
        {
            try
            {
                var noteDtoList = new List<NoteDto>();

                var notes = await _repository.GetAllAsync();

                foreach (Note note in notes)
                {
                    var noteDto = NoteMapper.ToDto(note);
                    noteDtoList.Add(noteDto);
                }
                return noteDtoList;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Service Error: Failed to retrieve all notes.");
                throw;
            }
        }

        // GET NOTE BY ID
        public async Task<NoteDto> GetNoteByIdAsync(Guid id)
        {
            try
            {
                var note = await _repository.GetByIdAsync(id);
                var noteDto = NoteMapper.ToDto(note);
                return noteDto;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Service Error: Failed to retrieve note with id {NoteId}", id);
                throw;
            }
        }

        // UPDATE NOTE BY ID
        public async Task UpdateNoteAsync(NoteDto noteDto)
        {
            try
            {
                var note = NoteMapper.ToEntity(noteDto);
                await _repository.UpdateAsync(note);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Service Error: Failed to update note with id {NoteId}", noteDto.Id);
                throw;
            }
        }
    }
}
