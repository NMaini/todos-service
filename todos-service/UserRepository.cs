using Dapper;
using Npgsql;

namespace todos_service;

public class UserRepository: IUsersRepository
{
    private readonly IConfiguration _configuration;
    
    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<User> GetUser(string userId)
    {
        var sqlQuery = "SELECT * From users as U WHERE U.Id = @userId";
        await using var connection = new NpgsqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        var users = await connection.QueryAsync<User>(sqlQuery, new { userId });
        return !users.Any() ? null : users.FirstOrDefault();
    }

    public async Task<bool> AddUser(User user)
    {
        var sqlCommand = "INSERT INTO users VALUES (@id, @password)";
        await using var connection = new NpgsqlConnection(_configuration.GetConnectionString("default"));
        await connection.OpenAsync();

        var result = await connection.ExecuteAsync(sqlCommand, new { id = user.Id, password = user.Password });
        return result == 1;
    }
}

public class User
{
    public string Id { get; set; }
    public string Password { get; set; }
}