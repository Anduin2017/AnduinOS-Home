using Microsoft.AspNetCore.Identity;

namespace Anduin.AnduinOSHome.Models.RolesViewModels;

public class IdentityRoleWithCount
{
    public required IdentityRole Role { get; init; }
    public required int UserCount { get; init; }
    public required int PermissionCount { get; init; }
    public required List<string> PermissionNames { get; init; }
}
