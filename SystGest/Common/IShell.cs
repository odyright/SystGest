using System;

using Windows.UI.Xaml.Controls;

namespace SystGest.UWP
{
    public interface IShell
    {
        Frame NavigationFrame { get; }
        NavigationViewItem SettingsItem { get; }
    }
}
