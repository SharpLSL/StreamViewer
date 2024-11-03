#if false
using System;
using System.Reflection;

using Avalonia.Controls;
using HanumanInstitute.MvvmDialogs;
#endif
using HanumanInstitute.MvvmDialogs.Avalonia;

namespace StreamViewer;

public class ViewLocator : ViewLocatorBase
{
    protected override string GetViewName(object viewModel) =>
        viewModel.GetType().FullName!
            .Replace(".ViewModels.", ".Views.")
            .Replace("ViewModel", "");

#if false
    public override ViewDefinition Locate(object viewModel)
    {
        var viewName = GetViewName(viewModel);
        var viewType = Assembly.GetExecutingAssembly()?.GetType(viewName);
        if (viewType == null ||
            (!typeof(Control).IsAssignableFrom(viewType) && !typeof(Window).IsAssignableFrom(viewType) && !typeof(IView).IsAssignableFrom(viewType)))
        {
            throw new TypeLoadException(
                string.Concat("Dialog view of type " +
                viewName +
                " for view model " +
                viewModel.GetType().FullName +
                " is missing.", Environment.NewLine,
                "Avalonia project template includes ViewLocator in the project base. You can customize it to map your view models to your views."));
        }

        return new ViewDefinition(viewType, () => CreateViewInstance(viewType));
    }
#endif
}


// References:
// [For Avalonia, allow for view models that are in a different assembly from views](https://github.com/mysteryx93/HanumanInstitute.MvvmDialogs/issues/34)
