using System;
using System.Collections.Generic;
using System.Reflection;

using CommunityToolkit.Mvvm.ComponentModel;
using HanumanInstitute.MvvmDialogs;
using SharpLSL;

namespace StreamViewer.ViewModels;

public partial class AboutWindowViewModel
    : ViewModelBase, IModalDialogViewModel, IViewLoaded
{
    [ObservableProperty]
    private string? version;

    [ObservableProperty]
    private string? sharpLSLVersion;

    [ObservableProperty]
    private string? libLSLVersion;

    [ObservableProperty]
    private string? libLSLBuildInfo;

    [ObservableProperty]
    private SortedList<string, string?>? referencedAssemblies;

    public bool? DialogResult { get; }

    public void OnLoaded()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

        var gitVersionInformationType = assembly?.GetType("GitVersionInformation");
        if (gitVersionInformationType != null)
        {
            var majorMinorPatchField = gitVersionInformationType.GetField("MajorMinorPatch");
            var shortShaField = gitVersionInformationType.GetField("ShortSha");

            var majorMinorPatch = majorMinorPatchField?.GetValue(null);
            var shortSha = shortShaField?.GetValue(null);

            if (majorMinorPatch != null && shortSha != null)
            {
                Version = $"v{majorMinorPatch}+{shortSha}";
            }
            else if (majorMinorPatch != null)
            {
                Version = $"v{majorMinorPatch}";
            }
        }

        var referencedAssemblies = new SortedList<string, string?>();

        var referencedAssembliesArray = assembly?.GetReferencedAssemblies() ?? Array.Empty<AssemblyName>();
        foreach (var referencedAssembly in referencedAssembliesArray)
        {
            if (referencedAssembly!.Name! != "SharpLSL")
            {
                if (!referencedAssemblies.ContainsKey(referencedAssembly!.Name!))
                {
                    referencedAssemblies.Add(
                        referencedAssembly!.Name!,
                        referencedAssembly.Version?.ToString());
                }
            }
            else
            {
                SharpLSLVersion = referencedAssembly?.Version?.ToString();
            }
        }

        var liblslVersion = LSL.GetLibraryVersion();
        LibLSLVersion = $"{liblslVersion / 100}.{liblslVersion % 100}";
        LibLSLBuildInfo = $"Build: {LSL.GetLibraryInfo()}\r\nProtocol version: {LSL.GetProtocolVersion()}";

        ReferencedAssemblies = referencedAssemblies;
    }
}


// References:
// https://github.com/DaxStudio/DaxStudio/blob/master/src/DaxStudio.UI/ViewModels/HelpAboutViewModel.cs
