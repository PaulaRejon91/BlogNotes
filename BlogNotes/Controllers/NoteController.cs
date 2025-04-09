using BlogNotes.Interfaces;
using BlogNotes.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogNotes.Controllers;

[ApiController]
[Route("[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly ILogger<NoteController> _logger;

    public NoteController(INoteService noteService, ILogger<NoteController> logger)
    {
        _noteService = noteService;
        _logger = logger;
    }

    // GET ALL NOTES
    [HttpGet(Name = "GetAllNotes")]
    public async Task<IActionResult> GetNotes()
    {
        IActionResult result;
        try
        {
            _logger.LogInformation("Received request to retrieve all notes.");
            var notes = await _noteService.GetAllNotesAsync();
            _logger.LogInformation("Successfully retrieved {Count} notes.", notes?.Count() ?? 0);
            result = Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Controller Error: Failed to retrieve all notes.");
            result = StatusCode(500, "An internal server error occurred while processing your request.");
        }
        return result;
    }

    // GET NOTE BY ID
    [HttpGet("{id}", Name = "GetNoteById")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        IActionResult result;
        try
        {
            _logger.LogInformation("Received request to retrieve note with id {NoteId}.", id);
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                _logger.LogWarning("Note with id {NoteId} not found.", id);
                result = NotFound();
            }
            else
            {
                _logger.LogInformation("Successfully retrieved note with id {NoteId}.", id);
                result = Ok(note);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Controller Error: Failed to retrieve note with id {NoteId}.", id);
            result = StatusCode(500, "An internal server error occurred while processing your request.");
        }
        return result;
    }

    // CREATE NOTE
    [HttpPost(Name = "PostNote")]
    public async Task<IActionResult> CreateNote([FromBody] NoteDto noteDto)
    {
        IActionResult result;
        try
        {
            if (noteDto == null)
            {
                _logger.LogWarning("CreateNote request received with null noteDto.");
                result = BadRequest("The note is null.");
            }
            else
            {
                await _noteService.CreateNoteAsync(noteDto);
                result = Accepted();
            }
        }
        catch (Exception ex)
        {
            result = StatusCode(500, "An internal server error occurred while processing your request.");
        }
        return result;
    }

    // UPDATE NOTE
    [HttpPut("{id}", Name = "UpdateNoteById")]
    public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteDto noteDto)
    {
        IActionResult result;
        try
        {
            if (noteDto == null || id != noteDto.Id)
            {
                _logger.LogWarning("UpdateNote request received with mismatched ids. Route id: {RouteId}, noteDto id: {NoteDtoId}", id, noteDto?.Id);
                result = BadRequest("The note ID does not match.");
            }
            else
            {
                _logger.LogInformation("Received request to update note with id {NoteId}.", id);
                await _noteService.UpdateNoteAsync(noteDto);
                _logger.LogInformation("Successfully updated note with id {NoteId}.", id);
                result = NoContent();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Controller Error: Failed to update note with id {NoteId}.", id);
            result = StatusCode(500, "An internal server error occurred while processing your request.");
        }
        return result;
    }

    // DELETE NOTE
    [HttpDelete("{id}", Name = "DeleteNoteById")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        IActionResult result;
        try
        {
            _logger.LogInformation("Received request to delete note with id {NoteId}.", id);
            await _noteService.DeleteNoteAsync(id);
            _logger.LogInformation("Successfully deleted note with id {NoteId}.", id);
            result = NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Controller Error: Failed to delete note with id {NoteId}.", id);
            result = StatusCode(500, "An internal server error occurred while processing your request.");
        }
        return result;
    }
}
