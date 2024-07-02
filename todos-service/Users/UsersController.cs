using Microsoft.AspNetCore.Mvc;
using todos_service.Core;

namespace todos_service;

[ApiController]
[Route("api/Auth")]
public class UsersController: ControllerBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly IAuthService _authService;

    public UsersController(IUsersRepository usersRepository, IAuthService authService)
    {
        _usersRepository = usersRepository;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IResult> RegisterUser([FromBody]User user)
    {
        if (!await _usersRepository.AddUser(user))
            return Results.Problem();

        return Results.Created();
    }

    [HttpPost("login")]
    public async Task<IResult> Login([FromBody]User userForm)
    {
        var user = await _usersRepository.GetUser(userForm.Id);
        if (user == null)
            return Results.BadRequest($"User with id {userForm.Id} not found");

        if (userForm.Password == user.Password)
        {
            var token = _authService.GenerateToken(userForm.Id);
            return Results.Ok(new { token });
        }

        return Results.BadRequest("Invalid Password");
    }
}