using Entities;

namespace ToDoList.Repos;

public interface IUserRepo
{ 
    ICollection<User> GetUsers();
    User GetUser(int userId);
    bool UserExist(int userId);
    bool CreateUser(User user);
}