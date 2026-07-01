namespace Identity.Application.Common.Authentication;

public interface IRefreshTokenGenerator
{
    string Generate();
}