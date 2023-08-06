using DataAccess;
using Entities;
using ToDoList.Dto;

namespace ToDoList.Repos;

public class NoteRepo:INoteRepo
{
    private readonly DataContext _dataContext;

    public NoteRepo(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public ICollection<Note> GetNotes()
    {
        return _dataContext.Notes.OrderBy(x => x.noteId).ToList();
    }

    public Note GetNote(int id)
    {
        return _dataContext.Notes.Where(x => x.noteId == id).FirstOrDefault();
    }

    public List<Note> NoteIsCompleted(bool completed)
    {
        return _dataContext.Notes.Where(x => x.IsCompleted==completed).ToList();
    }

    public bool NoteExist(int id)
    {
        return _dataContext.Notes.Any(x => x.noteId == id);
    }

    
    public bool PostNote(Note note, int userId)
    {
        var noteOwnerEntity = _dataContext.Users.Where(x => x.userId == userId).FirstOrDefault();
        var noteOwner = new NoteOwner()
        {
            User = noteOwnerEntity,
            Note=note
        };
        _dataContext.Add(noteOwner);
        _dataContext.Add(note);
        return Save();
    }

    public Note GetNoteTrimToUpper(NoteDto noteDto)
    {
        return GetNotes().Where(x => x.Content.Trim().ToUpper() == noteDto.Content.TrimEnd().ToUpper())
            .FirstOrDefault();
    }

    private bool Save()
    {
        var saved = _dataContext.SaveChanges();
        return saved > 0 ? true : false;
    }
}