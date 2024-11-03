using System;
using System.IO;
using System.Reflection;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

using StreamViewer.ViewModels;

namespace StreamViewer;

public partial class App : Application
{
    public override void Initialize()
    {
        var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var isDevelopment = environmentName == "Development";

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, isDevelopment)
            .AddJsonFile($"appsettings.{environmentName}.json", true, isDevelopment);

        var logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");

#pragma warning disable CA1305
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            //.MinimumLevel.Debug()
            //.WriteTo.File(Path.Combine(logDirectory, $"StreamViewer-{DateTime.Today:yyyy-MM-dd}.log"), rollingInterval: RollingInterval.Day)
            .WriteTo.File(Path.Combine(logDirectory, $"StreamViewer.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();
#pragma warning restore CA1305

        Log.Information("=================================== Startup ====================================");

        var host = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices(ConfigureServices)
            .Build();

        Ioc.Default.ConfigureServices(host.Services);

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Exit += OnExit;

            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            var dialogService = Ioc.Default.GetService<IDialogService>();
            var mainWindow = Ioc.Default.GetService<MainWindowViewModel>();
            dialogService!.Show(null, mainWindow!);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(HostBuilderContext _, IServiceCollection services)
    {
        var loggerFactory = LoggerFactory.Create(
            builder => builder.AddFilter(logLevel => true));

        services.AddSingleton<IDialogService>(
            new DialogService(
                new DialogManager(
                    viewLocator: new ViewLocator(),
                    logger: loggerFactory.CreateLogger<DialogManager>()),
                    viewModelFactory: x => Ioc.Default.GetService(x)));

        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        var baseDirectory = Path.GetDirectoryName(assembly!.Location);
        var settingsFilePath = Path.Combine(baseDirectory!, "Settings.json");

        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<WaveformControlViewModel>();
        services.AddTransient<SelectStreamWindowViewModel>();
        services.AddTransient<SettingsWindowViewModel>();
        services.AddTransient<AboutWindowViewModel>();
        services.AddSingleton(Settings.Load(settingsFilePath));
    }

    private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        Ioc.Default.GetService<Settings>()!.Store();

        Log.Information("=================================== Shutdown ===================================");
    }
}

// References:
// https://github.com/mysteryx93/HanumanInstitute.MvvmDialogs/tree/master/samples/Avalonia/Demo.ModalDialog
// [Ioc (Inversion of control)](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/ioc)
// https://github.com/CommunityToolkit/MVVM-Samples
// https://github.com/CommunityToolkit/MVVM-Samples/blob/master/samples/MvvmSampleXF/MvvmSampleXF/App.xaml.cs
// https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Mvvm/DependencyInjection/Ioc.cs
// [Call method on application exit in Avalonia](https://stackoverflow.com/questions/75247536/call-method-on-application-exit-in-avalonia)
// [CA1305 triggers for methods with overload where IFormatProvider parameter is optional](https://github.com/dotnet/roslyn-analyzers/issues/6255)
// [What is the difference between AddSerilog and UseSerilog in .NET 6 Web API?](https://stackoverflow.com/questions/73997580/what-is-the-difference-between-addserilog-and-useserilog-in-net-6-web-api)
// [.NET Core - Serilog - IHostBuilder does not contain a definiton of UseSerilog()](https://stackoverflow.com/questions/64678766/net-core-serilog-ihostbuilder-does-not-contain-a-definiton-of-useserilog)
// [How to get Serilog to work with Dependency Injection?](https://stackoverflow.com/questions/64865635/how-to-get-serilog-to-work-with-dependency-injection)
// [Serilog in WPF project](https://stackoverflow.com/questions/64546732/serilog-in-wpf-project)
// https://github.com/serilog/serilog/wiki/Getting-Started
// https://github.com/serilog/serilog/wiki/AppSettings
// [Configuring Serilog RollingFile with appsettings.json](https://stackoverflow.com/questions/40880261/configuring-serilog-rollingfile-with-appsettings-json)
// [Serilog using appsettings.json instead of appsettings.Development.json](https://stackoverflow.com/questions/68672903/serilog-using-appsettings-json-instead-of-appsettings-development-json)
// [How to dependency inject Serilog into the rest of my classes in .NET Console App](https://stackoverflow.com/questions/66304596/how-to-dependency-inject-serilog-into-the-rest-of-my-classes-in-net-console-app)
// [Serilog dependency injection](https://stackoverflow.com/questions/64280467/serilog-dependency-injection)
