using BlogNotes.Interfaces;
using BlogNotes.Models;
using BlogNotes.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BlogNotes.Tests.Services
{
    [TestFixture]
    public class NoteServiceTests
    {
        private INoteRepository _repository;
        private ILogger<NoteService> _logger;
        private NoteService _service;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<INoteRepository>();
            _logger = Substitute.For<ILogger<NoteService>>();
            _service = new NoteService(_repository, _logger);
        }

        [Test]
        public async Task CreateNoteAsync_Should_Call_AddAsync_OnRepository()
        {
            // Arrange
            var noteDto = new NoteDto
            {
                Id = Guid.NewGuid(),
                Title = "Test Note",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _service.CreateNoteAsync(noteDto);

            // Assert
            await _repository.Received(1).AddAsync(
                Arg.Is<Note>(n =>
                    n.Id == noteDto.Id &&
                    n.Title == noteDto.Title &&
                    n.Content == noteDto.Content &&
                    n.CreatedAt == noteDto.CreatedAt));
        }

        [Test]
        public async Task GetAllNotesAsync_Should_Return_ListOfNoteDtos()
        {
            // Arrange
            var notes = new List<Note>
            {
                new Note { Id = Guid.NewGuid(), Title = "Note 1", Content = "Content 1", CreatedAt = DateTime.UtcNow },
                new Note { Id = Guid.NewGuid(), Title = "Note 2", Content = "Content 2", CreatedAt = DateTime.UtcNow }
            };

            _repository.GetAllAsync().Returns(notes);

            // Act
            var result = await _service.GetAllNotesAsync();

            // Assert
            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(notes.Count));
            foreach (var note in notes)
            {
                Assert.IsTrue(resultList.Any(dto => dto.Id == note.Id));
            }
        }

        [Test]
        public async Task GetNoteByIdAsync_Should_Return_Correct_NoteDto()
        {
            // Arrange
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Note Test",
                Content = "Content Test",
                CreatedAt = DateTime.UtcNow
            };

            _repository.GetByIdAsync(note.Id).Returns(note);

            // Act
            var result = await _service.GetNoteByIdAsync(note.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(note.Id));
            Assert.That(result.Title, Is.EqualTo(note.Title));
            Assert.That(result.Content, Is.EqualTo(note.Content));
        }

        [Test]
        public async Task UpdateNoteAsync_Should_Call_UpdateAsync_OnRepository()
        {
            // Arrange
            var noteDto = new NoteDto
            {
                Id = Guid.NewGuid(),
                Title = "Updated Note",
                Content = "Updated Content",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _service.UpdateNoteAsync(noteDto);

            // Assert
            await _repository.Received(1).UpdateAsync(
                Arg.Is<Note>(n =>
                    n.Id == noteDto.Id &&
                    n.Title == noteDto.Title &&
                    n.Content == noteDto.Content));
        }

        [Test]
        public async Task DeleteNoteAsync_Should_Call_DeleteAsync_OnRepository()
        {
            // Arrange
            var noteId = Guid.NewGuid();

            // Act
            await _service.DeleteNoteAsync(noteId);

            // Assert
            await _repository.Received(1).DeleteAsync(noteId);
        }
    }
}
