using Anduin.AnduinOSHome.Entities;

namespace Anduin.AnduinOSHome.Models.UsersViewModels;

public class UserWithRolesViewModel
{
    public required User User { get; set; }
    public required IList<string> Roles { get; set; }
}