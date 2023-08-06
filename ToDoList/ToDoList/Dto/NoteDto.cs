namespace ToDoList.Dto;

public class NoteDto
{
    public int noteId { get; set; }
    public string Content { get; set; } = null!;
    public bool IsCompleted { get; set; }
}