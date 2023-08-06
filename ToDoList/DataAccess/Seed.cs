using Entities;

namespace DataAccess;

public class Seed
{
    private readonly DataContext _dataContext;

    public Seed(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public void DataSeed()
    {
        if (_dataContext.NoteOwners.Any())
        {
            var noteOwner = new List<NoteOwner>
            {
                new NoteOwner()
                {
                    Note = new Note()
                    {
                        Content = "my first note"
                    },
                    User = new User()
                    {
                        Email = "123@mail.com",
                        Password = "123add",
                    }
                }
            };
            _dataContext.NoteOwners.AddRange(noteOwner);
            _dataContext.SaveChanges();
        }
    }
}