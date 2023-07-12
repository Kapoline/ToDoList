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
        if (_dataContext.Users.Any())
        {
            var user = new List<User>()
            {
                new User()
                {
                    UserName = "polina",
                    Email = "pol@gmail.com",
                    Password = "pds12e",
                    Note = new List<Note>()
                    {
                        new Note()
                        {
                            Content = "my first note",
                            IsCompleted = false
                        }
                    }
                }

            };
            _dataContext.Users.AddRange(user);
            _dataContext.SaveChanges();
        }
    }
}