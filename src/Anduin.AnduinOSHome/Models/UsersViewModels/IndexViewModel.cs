using Aiursoft.UiStack.Layout;

namespace Anduin.AnduinOSHome.Models.UsersViewModels;

public class IndexViewModel : UiStackLayoutViewModel
{
    public IndexViewModel()
    {
        PageTitle = "Users";
    }

    public required List<UserWithRolesViewModel> Users { get; set; }
}
