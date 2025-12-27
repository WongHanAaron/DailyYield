using DailyYield.Domain.Entities;

namespace DailyYield.Domain.Ports;

public interface IAuthService
{
    Task<string> GenerateJwtToken(User user);
    Task<User?> ValidateUser(string email, string password);
}