namespace Entities;

public class NoteOwner
{
    public int NoteId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public Note Note { get; set; }
}