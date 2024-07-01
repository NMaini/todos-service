using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace todos_service;

[ApiController]
[Route("api/[controller]")]
public class TodosController: ControllerBase
{
    private readonly ITodosRepository _todosRepository;
    
    public TodosController(ITodosRepository todosRepository)
    {
        _todosRepository = todosRepository;
    }
    
    [HttpGet("/{userId}")]
    public async Task<IResult> GetTodos([FromRoute]string userId)
    {
        var todos = await _todosRepository.GetTodosForUser(userId);
        return Results.Ok(todos);
    }

    [HttpPost("/{userId}")]
    public async Task<IResult> AddTodo([FromRoute] string userId, [FromBody] string name)
    {
        try
        {
            var todo = new Todo()
            {
                Name = name,
                userId = userId,
                TodoIndex = await _todosRepository.GetTodosCount(userId) + 1,
                Status = "incomplete"
            };
            
            if (await _todosRepository.AddTodoForUser(todo))
                return Results.Created();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Results.BadRequest("Unable to add Todo");
    }
}