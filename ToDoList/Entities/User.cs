﻿namespace Entities;

public class User
{
    public int userId {get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<NoteOwner> NoteOwners { get; set; }
}