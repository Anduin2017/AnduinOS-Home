using Anduin.AnduinOSHome.Entities;
using Aiursoft.UiStack.Layout;

namespace Anduin.AnduinOSHome.Models.UsersViewModels;

public class DeleteViewModel : UiStackLayoutViewModel
{
    public DeleteViewModel()
    {
        PageTitle = "Delete User";
    }

    public required User User { get; set; }
}
