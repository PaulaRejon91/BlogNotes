using BlogNotes.Interfaces;
using BlogNotes.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogNotes.Controllers;

[ApiController]
[Route("[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    //GET ALL NOTES
    [HttpGet(Name = "GetAllNotes")]
    public async Task<IActionResult> GetNotes()
    {
        ObjectResult result;
        try
        {
            var notes = await _noteService.GetAllNotesAsync();
            result = Ok(notes);
        }
        catch (Exception ex)
        {
            result = BadRequest(ex);
        }

        return result;
    }

    //GET NOTE BY ID
    [HttpGet("{id}", Name = "GetNoteById")]
    public async Task<IActionResult> GetNoteById(Guid id)
    {
        ActionResult result;
        try
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(note);
            }
        }
        catch (Exception ex)
        {
            result = BadRequest(ex);
        }
        return result;
    }

    //CREATE NOTE
    [HttpPost(Name = "PostNote")]
    public async Task<IActionResult> CreateNote([FromBody] NoteDto noteDto)
    {
        ActionResult result;
        try
        {
            if (noteDto == null)
            {
                result = BadRequest("The note is null");
            }
            else
            {
                await _noteService.CreateNoteAsync(noteDto);

                result = CreatedAtAction(nameof(GetNoteById), new { id = noteDto.Id }, noteDto);
            }
        }
        catch (Exception ex)
        {
            result = BadRequest(ex);
        }
        return result;
    }

    //UPDATE NOTE
    [HttpPut("{id}", Name = "UpdateNoteById")]
    public async Task<IActionResult> UpdateNote(Guid id, [FromBody] NoteDto noteDto)
    {
        ActionResult result;
        try
        {
            if (noteDto == null || id != noteDto.Id)
            {
                result = BadRequest("The note ID does not match.");
            }
            else
            {
                await _noteService.UpdateNoteAsync(noteDto);
                result = NoContent();
            }
        }
        catch (Exception ex)
        {
            result = BadRequest(ex);
        }
        return result;
    }

    //DELETE NOTE
    [HttpDelete("{id}", Name = "DeleteNoteById")]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        ActionResult result;
        try
        {
            await _noteService.DeleteNoteAsync(id);
            result = NoContent();
        }
        catch (Exception ex)
        {
            result = BadRequest(ex);
        }
        return result;
    }
}
