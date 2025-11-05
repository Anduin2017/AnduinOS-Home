using Anduin.AnduinOSHome.Authorization;
using Anduin.AnduinOSHome.Entities;
using Aiursoft.UiStack.Layout;
using Microsoft.AspNetCore.Identity;

namespace Anduin.AnduinOSHome.Models.UsersViewModels;

public class DetailsViewModel : UiStackLayoutViewModel
{
    public DetailsViewModel()
    {
        PageTitle = "User Details";
    }

    public required User User { get; set; }

    public required IList<IdentityRole> Roles { get; set; }

    public required List<PermissionDescriptor> Permissions { get; set; }
}
