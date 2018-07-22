using System;

using SystGest.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SystGest.Views
{
    public sealed partial class DashBoardPage : Page
    {
        private DashBoardViewModel ViewModel
        {
            get { return DataContext as DashBoardViewModel; }
        }

        public DashBoardPage()
        {
            InitializeComponent();
        }
    }
}
