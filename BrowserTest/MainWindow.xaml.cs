// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using WinUICommunity;
using BrowserTest.Pages;
using DispatcherQueueHandler = Microsoft.UI.Dispatching.DispatcherQueueHandler;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BrowserTest
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private AppWindow m_AppWindow;
        public MainWindow()
        {
            this.InitializeComponent();
            m_AppWindow = this.AppWindow;
            m_AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            m_AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            m_AppWindow.Resize(new SizeInt32(1600, 900));
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            Tabs.TabItemsChanged += Tabs_TabItemsChanged;
            SetTitleBar(CustomDragRegion);
            CustomDragRegion.MinWidth = 188;
            SetupWindow();
        }

        private void Tabs_TabItemsChanged(TabView sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
        {
            // If there are no more tabs, close the window.
            if (sender.TabItems.Count == 0)
            {
                this.Close();
            }
            // If there is only one tab left, disable dragging and reordering of Tabs.
            else if (sender.TabItems.Count == 1)
            {
                sender.CanReorderTabs = false;
                sender.CanDragTabs = false;
            }
            else
            {
                sender.CanReorderTabs = true;
                sender.CanDragTabs = true;
            }
        }

        void SetupWindow()
        {
            Tabs.TabItems.Add(new TabViewItem() { IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Placeholder }, Header = $"Item", Content = new WebviewPage() });
            Tabs.SelectedIndex = 0;
        }

        public void AddTabToTabs(TabViewItem tab)
        {
            Tabs.TabItems.Add(tab);
        }

        private TabViewItem CreateNewTVI(string header, string dataContext)
        {
            var newTab = new TabViewItem()
            {
                IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource()
                {
                    Symbol = Symbol.Placeholder
                },
                Header = header,
                Content = new WebviewPage()
            };

            return newTab;
        }

        // This method prevents the TabView from handling things that aren't text (ie. files, images, etc.)
        private void Tabs_TabStripDragOver(object sender, DragEventArgs e)
        {
            
        }

        private void Tabs_AddTabButtonClick(TabView sender, object args)
        {
            var tab = CreateNewTVI("New Item", "New Item");
            sender.TabItems.Add(tab);
        }

        private void Tabs_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }
    }
}
