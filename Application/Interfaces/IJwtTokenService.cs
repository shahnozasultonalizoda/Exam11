using Domain.Entities;

namespace Application.Interfaces;

public interface IJwtTokenService
{
     Task<string> GenerateToken(User user);
}
