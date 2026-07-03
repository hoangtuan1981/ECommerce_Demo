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
        throw new NotImplementedException();
    }
}