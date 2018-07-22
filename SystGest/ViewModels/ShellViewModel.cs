using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using CommonServiceLocator;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SystGest.Authentication;
using SystGest.Helpers;
using SystGest.Services;
using SystGest.UWP;
using SystGest.Views;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SystGest.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private NavigationView _navigationView;
        private NavigationViewItem _selected;
        private ICommand _itemInvokedCommand;
        static public ShellViewModel Current { get; private set; }
        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();
        public NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        private Contact _userContact = new Contact();
        public Contact UserContact
        {
            get { return _userContact; }
            set { Set(ref _userContact, value); }
        }
        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));
        public ICommand LogoutCommand => new RelayCommand(OnLogout);
        public ShellViewModel()
        {
            Current = this;
        }
        private IShell Shell { get; set; }
        public async void Initialize(IShell shell, ActivationState activationState)
        {
            Shell = shell;
            NavigationService.Frame = Shell.NavigationFrame;
            NavigationService.Navigated += OnNavigated;
            NavigationService.Navigate(activationState.ViewModel.ToString(), activationState.Parameter);

            UserContact = await GetUserContactAsync();
        }
        //public async void Initialize(Frame frame, NavigationView navigationView)
        //{
        //    _navigationView = navigationView;
        //    NavigationService.Frame = frame;
        //    NavigationService.Navigated += OnNavigated;
        //    UserContact = await GetUserContactAsync();
        //}

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsViewModel).FullName);
                return;
            }

            var item = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
            NavigationService.Navigate(pageKey);
        }
        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as NavigationViewItem;
                return;
            }

            Selected = _navigationView.MenuItems
                            .OfType<NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }
        private async Task<Contact> GetUserContactAsync()
        {
            return await ContactHelper.CreateContactFromLogonUserAsync();
        }

        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var navigatedPageKey = NavigationService.GetNameOfRegisteredPage(sourcePageType);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == navigatedPageKey;
        }
        private async void OnLogout()
        {
            if (await DialogBox.ShowAsync("Confirm Logout", "Are you sure you want to logout?", "Ok", "Cancel"))
            {
                NavigationService.GoBack();
            }
        }
    }
}
