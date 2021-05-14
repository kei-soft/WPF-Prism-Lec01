using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

using BlankCoreApp1.Views;

using Prism.Ioc;
using Prism.Mvvm;

namespace BlankCoreApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // View 단을 View 로 끝나게 한경우 아래 코딩 작업이 필요함
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName;
                if (viewName == null) return null;
                if (viewName.EndsWith("Page")) viewName = viewName.Substring(0, viewName.Length - 4);
                if (viewName.EndsWith("Control")) viewName = viewName.Substring(0, viewName.Length - 7);
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                viewName = viewName.Replace(".Controls.", ".ControlViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = $"{viewName}ViewModel,{ viewAssemblyName} ";

                return Type.GetType(viewModelName);
            });
        }
    }
}
