namespace todos_service;

public interface IUsersRepository
{
    public Task<bool> AddUser(User user);
    public Task<User> GetUser(string userId);
}