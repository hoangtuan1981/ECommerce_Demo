using Identity.Application.Common.Authentication;
using System.Security.Cryptography;

namespace Identity.Infrastructure.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}