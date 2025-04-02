using BlogNotes.Data;
using BlogNotes.Models;
using BlogNotes.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace BlogNotes.Tests.Repository
{
    [TestFixture]
    public class NoteRepositoryTests
    {
        private NotesDbContext _context;
        private ILogger<NoteRepository> _logger;
        private NoteRepository _repository;
        private DbContextOptions<NotesDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new NotesDbContext(_options);

            _logger = Substitute.For<ILogger<NoteRepository>>();

            _repository = new NoteRepository(_context, _logger);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_Should_Add_Note()
        {
            // Arrange
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Test Note",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            await _repository.AddAsync(note);

            // Assert
            var noteInDb = await _context.Notes.FindAsync(note.Id);
            Assert.IsNotNull(noteInDb);
            Assert.That(noteInDb.Title, Is.EqualTo(note.Title));
        }

        [Test]
        public async Task DeleteAsync_Should_Remove_Note()
        {
            // Arrange
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Test Note",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow
            };
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(note.Id);

            // Assert
            var noteInDb = await _context.Notes.FindAsync(note.Id);
            Assert.IsNull(noteInDb);
        }

        [Test]
        public async Task GetAllAsync_Should_Return_All_Notes()
        {
            // Arrange
            var note1 = new Note { Id = Guid.NewGuid(), Title = "Note 1", Content = "Content 1", CreatedAt = DateTime.UtcNow };
            var note2 = new Note { Id = Guid.NewGuid(), Title = "Note 2", Content = "Content 2", CreatedAt = DateTime.UtcNow };
            await _context.Notes.AddRangeAsync(note1, note2);
            await _context.SaveChangesAsync();

            // Act
            var notes = await _repository.GetAllAsync();

            // Assert
            Assert.That(notes.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_Should_Return_Correct_Note()
        {
            // Arrange
            var note = new Note { Id = Guid.NewGuid(), Title = "Note Test", Content = "Content Test", CreatedAt = DateTime.UtcNow };
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(note.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Title, Is.EqualTo(note.Title));
        }

        [Test]
        public async Task UpdateAsync_Should_Modify_Note()
        {
            // Arrange
            var note = new Note { Id = Guid.NewGuid(), Title = "Note Test", Content = "Content Test", CreatedAt = DateTime.UtcNow };
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            // Act
            note.Title = "Updated Title";
            note.Content = "Updated Content";
            await _repository.UpdateAsync(note);

            // Assert
            var updatedNote = await _context.Notes.FindAsync(note.Id);
            Assert.That(updatedNote.Title, Is.EqualTo("Updated Title"));
            Assert.That(updatedNote.Content, Is.EqualTo("Updated Content"));
        }

        [Test]
        public void AddAsync_Should_LogError_When_Exception_Occurs()
        {
            // Arrange
            var faultyContext = new FaultyNotesDbContext(_options);
            var errorRepository = new NoteRepository(faultyContext, _logger);

            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Faulty Note",
                Content = "Faulty Content",
                CreatedAt = DateTime.UtcNow
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await errorRepository.AddAsync(note));
        }

        // Simulated fault
        public class FaultyNotesDbContext : NotesDbContext
        {
            public FaultyNotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                throw new Exception("Simulated SaveChanges failure");
            }
        }
    }    
}
