using Identity.Application.Common.Authentication;
using Identity.Domain.Entities;
using System.Security.Cryptography;

namespace Identity.Infrastructure.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    //public string Generate()
    //{
    //    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    //}
    public GeneratedRefreshToken Generate()
    {
        var token = Convert.ToHexString(
        RandomNumberGenerator.GetBytes(64));

        return new GeneratedRefreshToken(
        token,
        DateTime.UtcNow.AddDays(7));
    }
}