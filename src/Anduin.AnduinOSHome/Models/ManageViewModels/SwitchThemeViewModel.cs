using System.ComponentModel.DataAnnotations;

namespace Anduin.AnduinOSHome.Models.ManageViewModels;

public class SwitchThemeViewModel
{
    [Required]
    public required string Theme { get; set; }
}
