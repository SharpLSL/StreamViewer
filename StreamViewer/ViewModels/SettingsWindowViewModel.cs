using HanumanInstitute.MvvmDialogs;

namespace StreamViewer.ViewModels;

public partial class SettingsWindowViewModel
    : ViewModelBase, IModalDialogViewModel
{
    public SettingsWindowViewModel(Settings settings) => Settings = settings;

    public Settings Settings { get; }

    public bool? DialogResult { get; }
}
