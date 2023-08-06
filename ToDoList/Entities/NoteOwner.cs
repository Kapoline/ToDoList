namespace Entities;

public class NoteOwner
{
    public int NoteOwnerId { get; set; }
    public int NoteId { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public Note Note { get; set; }
}