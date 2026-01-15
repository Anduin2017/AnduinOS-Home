using Aiursoft.UiStack.Layout;

namespace Anduin.AnduinOSHome.Models.BackgroundJobs;

public class JobsIndexViewModel : UiStackLayoutViewModel
{
    public IEnumerable<JobInfo> AllRecentJobs { get; init; } = [];
}
