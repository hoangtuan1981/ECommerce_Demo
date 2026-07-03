using Identity.Domain.Entities;

namespace Identity.Application.Common.Authentication;

public interface IRefreshTokenGenerator
{
    GeneratedRefreshToken Generate();
}