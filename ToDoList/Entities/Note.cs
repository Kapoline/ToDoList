using System.Collections;

namespace Entities;

public class Note
{
    public int noteId { get; set; }
    public string Content { get; set; }
    public bool IsCompleted { get; set; }
    public ICollection<NoteOwner> NoteOwners { get; set; }
}