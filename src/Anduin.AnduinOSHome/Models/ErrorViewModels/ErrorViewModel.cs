using Aiursoft.UiStack.Layout;

namespace Anduin.AnduinOSHome.Models.ErrorViewModels;

public class ErrorViewModel: UiStackLayoutViewModel
{
    public ErrorViewModel()
    {
        PageTitle = "Error";
    }

    public int ErrorCode { get; set; } = 500;

    public required string RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string? ReturnUrl { get; set; }
}
