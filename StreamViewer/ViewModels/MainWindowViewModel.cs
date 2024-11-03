using System;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using SharpLSL;

namespace StreamViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(Settings settings, IDialogService dialogService)
    {
        Settings = settings;

        this.dialogService = dialogService;

        Waveform = new WaveformControlViewModel();
    }

    public Settings Settings { get; }

    [ObservableProperty]
    private WaveformControlViewModel waveform;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConnectToStreamCommand))]
    private StreamInfo? selectedRegularStream;

    [RelayCommand]
    private async Task SelectStream()
    {
        using var selectStreamWindow = Ioc.Default.GetService<SelectStreamWindowViewModel>();
        var result = await dialogService.ShowDialogAsync(this, selectStreamWindow!);
        if (result == true)
        {
            SelectedRegularStream = selectStreamWindow!.SelectedRegularStream;
        }
    }

    [RelayCommand(CanExecute = nameof(CanConnectToStream))]
    private void ConnectToStream()
    {
        StreamInlet streamInlet;
        try
        {
            streamInlet = new StreamInlet(SelectedRegularStream);
        }
        catch (Exception ex)
        {
            return;
        }


    }

    private bool CanConnectToStream() => SelectedRegularStream != null;

    [RelayCommand]
    private async Task ShowSettingsWindow()
    {
        var settingsWindow = Ioc.Default.GetService<SettingsWindowViewModel>();
        await dialogService.ShowDialogAsync(this, settingsWindow!);
    }

    [RelayCommand]
    private async Task ShowAboutWindow()
    {
        var aboutWindow = Ioc.Default.GetService<AboutWindowViewModel>();
        await dialogService.ShowDialogAsync(this, aboutWindow!);
    }

    private readonly IDialogService dialogService;
}
