using Aiursoft.UiStack.Layout;

namespace Anduin.AnduinOSHome.Models.ErrorViewModels;

public class UnauthorizedViewModel: UiStackLayoutViewModel
{
    public UnauthorizedViewModel()
    {
        PageTitle = "Unauthorized";
    }

    public required string ReturnUrl { get; init; }
}
