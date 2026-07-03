chatGPT 
    https://chatgpt.com/share/6a47436d-2064-83ec-aa35-14c8540be40c
    
# Handler
    Identity.Application
    │
    ├── Abstractions
    │      IJwtProvider
    │      IPasswordHasher
    │      ICurrentUser
    │
    ├── Behaviors
    │      ValidationBehavior
    │      LoggingBehavior
    │
    ├── Common
    │      Result
    │      Error
    │      PagedResult
    │
    ├── DTOs
    │
    ├── Commands
    │
    ├── Queries
    │
    ├── Validators
    │
    ├── Mappings
    │
    └── DependencyInjection

# CQRS
    1. Authentication
        LoginCommand

        RefreshTokenCommand

        LogoutCommand

        ChangePasswordCommand

        ResetPasswordCommand

        ForgotPasswordCommand
    
    2. User
        1. command
            CreateUserCommand

            UpdateUserCommand

            DeleteUserCommand

            AssignRoleCommand

            RemoveRoleCommand
        2. Queries
            GetUserByIdQuery

            GetUsersQuery

            GetCurrentUserQuery

# Repository
    Application sẽ chỉ biết Interface.

# MediatR Pipeline



Features
└── Authentication
    ├── Login
    │   ├── LoginCommand.cs
    │   ├── LoginCommandHandler.cs
    │   ├── LoginCommandValidator.cs
    │   └── LoginResponse.cs
    │
    ├── RefreshToken
    │   ├── RefreshTokenCommand.cs
    │   ├── RefreshTokenCommandHandler.cs
    │   ├── RefreshTokenCommandValidator.cs
    │   └── RefreshTokenResponse.cs
    │
    ├── Logout
    │   ├── LogoutCommand.cs
    │   ├── LogoutCommandHandler.cs
    │   └── LogoutCommandValidator.cs
    │
    ├── ChangePassword
    │   ├── ChangePasswordCommand.cs
    │   ├── ChangePasswordCommandHandler.cs
    │   └── ChangePasswordCommandValidator.cs
    │
    ├── ForgotPassword
    │   ├── ForgotPasswordCommand.cs
    │   ├── ForgotPasswordCommandHandler.cs
    │   └── ForgotPasswordCommandValidator.cs
    │
    └── ResetPassword
        ├── ResetPasswordCommand.cs
        ├── ResetPasswordCommandHandler.cs
        └── ResetPasswordCommandValidator.cs

# FluentValidation là gì?
FluentValidation là một thư viện bên thứ ba (third-party library) mã nguồn mở dành cho .NET.