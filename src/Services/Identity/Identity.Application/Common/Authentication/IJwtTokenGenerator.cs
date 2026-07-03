using Identity.Domain.Entities;

namespace Identity.Application.Common.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
    //TokenResult Generate(User user);
    JwtToken Generate(User user);
}