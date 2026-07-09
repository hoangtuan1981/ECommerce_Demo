using Identity.Application.Common.Authentication;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Seed;

public sealed class DataSeeder
{
    private readonly IdentityDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        IdentityDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<DataSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        // Roles

        var adminRole = await GetOrCreateRoleAsync(
            "Admin",
            "System Administrator",
            cancellationToken);

        var sellerRole = await GetOrCreateRoleAsync(
            "Seller",
            "Store Seller",
            cancellationToken);

        var managerRole = await GetOrCreateRoleAsync(
            "Manager",
            "Store Manager",
            cancellationToken);

        var customerRole = await GetOrCreateRoleAsync(
            "Customer",
            "Default Customer",
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        // Users

        await CreateUserAsync(
            email: "admin@ecommerce.com",
            userName: "admin",
            fullName: "System Administrator",
            password: "Password@123",
            role: adminRole,
            cancellationToken);

        await CreateUserAsync(
            email: "seller@ecommerce.com",
            userName: "seller",
            fullName: "Default Seller",
            password: "Password@123",
            role: sellerRole,
            cancellationToken);

        await CreateUserAsync(
            email: "manager@ecommerce.com",
            userName: "manager",
            fullName: "Default Manager",
            password: "Password@123",
            role: managerRole,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Identity seed completed successfully.");
    }

    private async Task<Role> GetOrCreateRoleAsync(
        string roleName,
        string description,
        CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(
                x => x.Name == roleName,
                cancellationToken);

        if (role is not null)
            return role;

        role = Role.Create(
            roleName,
            description);

        await _context.Roles.AddAsync(
            role,
            cancellationToken);

        _logger.LogInformation(
            "Role {RoleName} created.",
            roleName);

        return role;
    }

    private async Task CreateUserAsync(
        string email,
        string userName,
        string fullName,
        string password,
        Role role,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);

        if (user == null)
        {
            user = User.Create(
                userName,
                email,
                _passwordHasher.Hash(password),
                fullName);

            await _context.Users.AddAsync(
                user,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            _logger.LogInformation(
                "User {Email} created.",
                email);
        }

        var userRoleExists = await _context.UserRoles
            .AnyAsync(
                x => x.UserId == user.Id &&
                     x.RoleId == role.Id,
                cancellationToken);

        if (!userRoleExists)
        {
            await _context.UserRoles.AddAsync(
                new UserRole(
                    user.Id,
                    role.Id),
                cancellationToken);

            _logger.LogInformation(
                "Role {RoleName} assigned to {Email}.",
                role.Name,
                email);
        }
    }
}