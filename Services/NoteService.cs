using BlogNotes.Interfaces;
using BlogNotes.Mappers.BlogNotes.Mappers;
using BlogNotes.Models;

namespace BlogNotes.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repository;

        public NoteService (INoteRepository repository)
        {
            _repository = repository;
        }

        //CREATE NOTE
        public async Task CreateNoteAsync(NoteDto noteDto)
        {
            var note = NoteMapper.ToEntity(noteDto);

            await _repository.AddAsync(note);
        }

        //DELETE NOTE
        public async Task DeleteNoteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        //GET ALL NOTES
        public async Task<IEnumerable<NoteDto>> GetAllNotesAsync()
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

        //GET NOTE BY ID
        public async Task<NoteDto> GetNoteByIdAsync(Guid id)
        {
            var note = await _repository.GetByIdAsync(id);
            var noteDto = NoteMapper.ToDto(note);
            return noteDto;
        }

        //UPDATE NOTE BY ID
        public async Task UpdateNoteAsync(NoteDto noteDto)
        {
            var note = NoteMapper.ToEntity(noteDto);
            await _repository.UpdateAsync(note);
        }
    }
}
