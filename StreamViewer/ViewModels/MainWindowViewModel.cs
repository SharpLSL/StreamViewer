using System;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using Microsoft.Extensions.Logging;

using SharpLSL;

namespace StreamViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(
        Settings settings,
        IDialogService dialogService,
        ILogger<MainWindowViewModel> logger)
    {
        Settings = settings;

        this.dialogService = dialogService;
        this.logger = logger;

        Waveform = Ioc.Default.GetService<WaveformControlViewModel>();
    }

    public Settings Settings { get; }

    [ObservableProperty]
    private WaveformControlViewModel? waveform;

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
            FailedToCreateStreamInlet(logger, ex);
            return;
        }


    }

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Failed to create StreamInlet!")]
    public static partial void FailedToCreateStreamInlet(ILogger logger, Exception ex);

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
    private readonly ILogger<MainWindowViewModel> logger;
}


// References:
// [Please provide an example for CA1848](https://github.com/dotnet/docs/issues/28306)
// [Compile-time logging source generation](https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator)
// [LoggerMessageAttribute extension to support Exception messages](https://github.com/dotnet/runtime/issues/84233)
// [Microsoft.Extensions.Logging, LoggerMessageAttribute, and Message Templates structure capturing operator](https://stackoverflow.com/questions/71664943/microsoft-extensions-logging-loggermessageattribute-and-message-templates-stru)
// [Enhance .NET Performance with Structured Logging using LoggerMessageAttribute](https://medium.com/@judevajira/enhance-net-performance-with-structured-logging-using-loggermessageattribute-d48a2a569590)
