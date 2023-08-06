using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DataAccess;
using Entities;
using Microsoft.IdentityModel.Tokens;

namespace ToDoList.Repos;

public class UserRepo: IUserRepo
{
    private readonly DataContext _dataContext;

    public UserRepo(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public ICollection<User> GetUsers()
    {
        return _dataContext.Users.OrderBy(x => x.UserName).ToList();
    }

    public User GetUser(int userId)
    {
        return _dataContext.Users.Where(x => x.userId == userId).FirstOrDefault();
    }

    public bool UserExist(int userId)
    {
        return _dataContext.Users.Any(x => x.userId == userId);
    }

    public bool CreateUser(User userdata)
    {
        _dataContext.Add(userdata);
        return Save();
    }
    private bool Save()
    {
        var saved = _dataContext.SaveChanges();
        return saved > 0 ? true : false;
    }
}