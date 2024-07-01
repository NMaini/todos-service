using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace todos_service;

public class TodosRepository: ITodosRepository
{
    private readonly IConfiguration _configuration;
    
    public TodosRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<Todo>> GetTodosForUser(string userId)
    {
        var connection = new NpgsqlConnection(_configuration.GetConnectionString("default"));
        var sqlQuery = "SELECT * From todos WHERE userId = @userId";
        await connection.OpenAsync();
        
        var todos = await connection.QueryAsync<Todo>(sqlQuery, new { userId });
        return todos;
    }

    public async Task<bool> AddTodoForUser(Todo todo)
    {
        var connection = new NpgsqlConnection(_configuration.GetConnectionString("default"));
        var sqlCommand = "INSERT INTO todos VALUES (@userId, @todoName, @todoIndex, @status)";
        await connection.OpenAsync();

        var result = await connection.ExecuteAsync(sqlCommand, new
        {
            userId = todo.userId,
            todoName = todo.Name,
            todoIndex = todo.TodoIndex,
            status = todo.Status
        });
        return result == 1;
    }

    public async Task<int> GetTodosCount(string userId)
    {
        var connection = new NpgsqlConnection(_configuration.GetConnectionString("default"));
        var sqlCommand = "SELECT * from todos WHERE userId=@userId";
        await connection.OpenAsync();

        var todos = await connection.QueryAsync<Todo>(sqlCommand, new { userId });
        return todos.Count();
    }
}