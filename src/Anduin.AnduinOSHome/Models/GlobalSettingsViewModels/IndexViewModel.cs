using Aiursoft.UiStack.Layout;

namespace Anduin.AnduinOSHome.Models.GlobalSettingsViewModels;

public class IndexViewModel : UiStackLayoutViewModel
{
    public IndexViewModel()
    {
        PageTitle = "Global Settings";
    }

    public List<SettingViewModel> Settings { get; set; } = new();
}
