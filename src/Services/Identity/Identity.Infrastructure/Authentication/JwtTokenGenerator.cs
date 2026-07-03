using Identity.Application.Common.Authentication;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public JwtToken Generate(User user)
    {
        throw new NotImplementedException();
    }
}
