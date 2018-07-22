using System;

using SystGest.Services;
using SystGest.UWP;
using SystGest.ViewModels;

using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SystGest.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page, IShell
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;
        public ShellPage()
        {
            InitializeComponent();
            HideNavViewBackButton();
            DataContext = ViewModel;
            //ViewModel.Initialize(shellFrame, navigationView);
            KeyboardAccelerators.Add(ActivationService.AltLeftKeyboardAccelerator);
            KeyboardAccelerators.Add(ActivationService.BackKeyboardAccelerator);
        }
        public Frame NavigationFrame => shellFrame;
        public NavigationViewItem SettingsItem => navigationView.SettingsItem as NavigationViewItem;
        private void HideNavViewBackButton()
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var activationState = e.Parameter as ActivationState;
            activationState = activationState ?? ActivationState.Default;
            ViewModel.Initialize(this, activationState);
            //ViewModel.Initialize(shellFrame, navigationView);
        }
        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    base.OnNavigatedFrom(e);
        //    ViewModel.UnregisterEvents();
        //}
    }
}
