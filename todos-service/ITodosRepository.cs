namespace todos_service;

public interface ITodosRepository
{
    public Task<IEnumerable<Todo>> GetTodosForUser(string userId);

    public Task<bool> AddTodoForUser(Todo todo);

    public Task<int> GetTodosCount(string userId);
}