namespace todos_service.Core;

public interface IAuthService
{
    public string GenerateToken(string userId);
}