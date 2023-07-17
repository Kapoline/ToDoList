using AutoMapper;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Dto;
using ToDoList.Repos;

namespace ToDoList.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NoteController:Controller
{
    private readonly DataContext _dataContext;
    private readonly INoteRepo _noteRepo;
    private readonly IMapper _mapper;

    public NoteController(DataContext dataContext, INoteRepo noteRepo, IMapper mapper)
    {
        _dataContext = dataContext;
        _noteRepo = noteRepo;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Note>))]
    public IActionResult GetNotes()
    {
        var notes = _mapper.Map<List<NoteDto>>(_noteRepo.GetNotes());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(notes);
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Note))]
    [ProducesResponseType(400)]
    public IActionResult GetNote(int id)
    {
        var note = _mapper.Map<NoteDto>(_noteRepo.GetNote(id));

        if (!_noteRepo.NoteExist(id))
            return NotFound();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(note);
    }

    [HttpGet("{noteId/completed}")]
    public IActionResult GetCompletedNotes()
    {
        var note = _mapper.Map<NoteDto>(_dataContext.Notes.Where(x => x.IsCompleted).ToListAsync());
        if (!ModelState.IsValid)
            return BadRequest();
        return Ok(note);
    }
    
    [HttpPost]
    public IActionResult PostNote([FromQuery] int userId, [FromBody] NoteDto noteDto)
    {
        if (noteDto == null)
            return BadRequest(ModelState);
        
        var notes = _noteRepo.GetNoteTrimToUpper(noteDto);

        if (noteDto != null)
        {
            ModelState.AddModelError("", "User already exist");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var noteMap = _mapper.Map<Note>(noteDto);

        if (!_noteRepo.PostNote(noteMap, userId))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Success");
    }
}