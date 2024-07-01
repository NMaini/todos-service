using Microsoft.AspNetCore.Mvc;

namespace todos_service;

[ApiController]
[Route("api/User")]
public class UsersController: ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpPost]
    public async Task<IResult> RegisterUser([FromBody]User user)
    {
        if (!await _usersRepository.AddUser(user))
            return Results.Problem();

        return Results.Created();
    }
}