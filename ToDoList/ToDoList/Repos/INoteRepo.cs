using Entities;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Dto;

namespace ToDoList.Repos;

public interface INoteRepo
{
    ICollection<Note> GetNotes();
    Note GetNote(int id);
    bool NoteExist(int id);
    bool PostNote(Note note, int userId);
    Note GetNoteTrimToUpper(NoteDto noteDto);
}