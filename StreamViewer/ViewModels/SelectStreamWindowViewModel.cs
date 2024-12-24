using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using SharpLSL;

namespace StreamViewer.ViewModels;

public partial class SelectStreamWindowViewModel
    : ViewModelBase, ICloseable, IDisposable, IModalDialogViewModel, IViewClosing
{
    public SelectStreamWindowViewModel()
    {
        continuousResolver = new ContinuousResolver();
        dispatcherTimer = new DispatcherTimer(
            TimeSpan.FromSeconds(1),
            DispatcherPriority.Normal,
            TimerCallback);
    }

    public event EventHandler? RequestClose;

    public void OnClosing(CancelEventArgs e) => dispatcherTimer.Stop();

    public async Task OnClosingAsync(CancelEventArgs e)
    {
        OnClosing(e);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                continuousResolver.Dispose();
            }

            disposed = true;
        }
    }

    [ObservableProperty]
    private IList<StreamInfo> regularStreams = Array.Empty<StreamInfo>();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AcceptCommand))]
    private StreamInfo? selectedRegularStream;

    [ObservableProperty]
    private IList<StreamInfo> irregularStreams = Array.Empty<StreamInfo>();

    [ObservableProperty]
    private bool? dialogResult;

    private void TimerCallback(object? sender, EventArgs e)
    {
        var streams = continuousResolver.Results();

        var selectedStream = SelectedRegularStream;

        RegularStreams = streams
            .Where(stream => stream.NominalSrate != LSL.IrregularRate)
            .ToList();

        if (selectedStream != null)
        {
            var matchedStream = RegularStreams
                .FirstOrDefault(stream => stream.Uid == selectedStream.Uid);
            if (matchedStream != null)
            {
                SelectedRegularStream = matchedStream;
            }
        }

        IrregularStreams = streams
            .Where(stream => stream.NominalSrate == LSL.IrregularRate)
            .ToList();
    }

    [RelayCommand(CanExecute = nameof(CanAccept))]
    private void Accept()
    {
        DialogResult = true;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private bool CanAccept() => SelectedRegularStream != null;

    [RelayCommand]
    private void Cancel()
    {
        DialogResult = false;
        RequestClose?.Invoke(this, EventArgs.Empty);
    }

    private readonly ContinuousResolver continuousResolver;
    private readonly DispatcherTimer dispatcherTimer;
    private bool disposed;
}


// References:
// [While disposing the class instance, do i need to dispose all its IDisposable members explicitly?](https://stackoverflow.com/questions/31783402/while-disposing-the-class-instance-do-i-need-to-dispose-all-its-idisposable-mem)
// https://github.com/mysteryx93/HanumanInstitute.MvvmDialogs/tree/master/samples/Avalonia/Demo.ModalDialog
// https://github.com/mysteryx93/HanumanInstitute.MvvmDialogs/blob/master/samples/Avalonia/Demo.ViewEvents/MainWindowViewModel.cs
// [Suppress warning from empty async method](https://stackoverflow.com/questions/21155692/suppress-warning-from-empty-async-method)
// [If my interface must return Task what is the best way to have a no-operation implementation?](https://stackoverflow.com/questions/13127177/if-my-interface-must-return-task-what-is-the-best-way-to-have-a-no-operation-imp)
