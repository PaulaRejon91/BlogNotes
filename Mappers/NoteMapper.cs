using BlogNotes.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BlogNotes.Mappers
{
    namespace BlogNotes.Mappers
    {
        public static class NoteMapper
        {
            public static NoteDto ToDto(Note note)
            {
                if (note == null) throw new ArgumentNullException(nameof(note));

                return new NoteDto
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt
                };
            }

            public static Note ToEntity(NoteDto noteDto)
            {
                if (noteDto == null) throw new ArgumentNullException(nameof(noteDto));

                return new Note
                {
                    Id = noteDto.Id ?? Guid.NewGuid(),
                    Title = noteDto.Title,
                    Content = noteDto.Content ?? string.Empty,
                    CreatedAt = noteDto.CreatedAt ?? DateTime.UtcNow,
                };
            }
        }
    }
}
