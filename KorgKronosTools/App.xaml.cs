// (c) Copyright 2011-2019 MiKeSoft, Michel Keijzers, All rights reserved

using System.Windows;
using PcgTools.PcgToolsResources;

namespace PcgTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] Arguments { get; private set;  }
        
        void Application_Startup(object sender, StartupEventArgs e)
        {
            // Apply dark background + Win32 dark title bar to every Window that opens in the app.
            EventManager.RegisterClassHandler(
                typeof(Window),
                FrameworkElement.LoadedEvent,
                new RoutedEventHandler(OnAnyWindowLoaded));

            if (e.Args.Length > 0)
                Arguments = e.Args;
        }

        private static void OnAnyWindowLoaded(object sender, RoutedEventArgs e)
        {
            DarkModeHelper.Apply(sender as Window);
        }

        /// <summary>
        /// Do not remove, although 0 references are mentioned, this method takes care of the
        /// resource regeneration problem, making a default public instead of internal constructor.
        /// There is also another method copied in the ResourceHelper.
        /// </summary>
        /// <returns></returns>
        public static Strings GetStringResources()
        {
            return new PcgToolsResources.Strings();
        }
    }
}
