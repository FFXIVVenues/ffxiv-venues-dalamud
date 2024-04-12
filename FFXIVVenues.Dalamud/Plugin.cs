using System;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.Net.Http;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using Dalamud.Interface.Windowing;
using FFXIVVenues.Dalamud.UI;
using Dalamud.Game.Command;
using System.Runtime.CompilerServices;
using ImGuiNET;
using System.Numerics;
using Dalamud.Logging;

namespace FFXIVVenues.Dalamud
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "FFXIV Venues";
        private readonly ServiceProvider _serviceProvider;
        public WindowSystem WindowSystem = new("FFXIVVenues");
        public VenueDirectoryWindow VenueDirectoryWindow { get; private set; }
        public WindowDetail WindowDetail { get; private set; }
        private const string CommandName = "/venues";
        public static Plugin _plugin;

        

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager,
            [RequiredVersion("1.0")] IChatGui chatGui)
        {
            _plugin = this;
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.ffxivvenues.com/");
            var config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(pluginInterface);
            serviceCollection.AddSingleton(pluginInterface.UiBuilder);
            serviceCollection.AddSingleton(commandManager);
            serviceCollection.AddSingleton(chatGui);
            serviceCollection.AddSingleton(config);
            serviceCollection.AddSingleton(httpClient);

           //serviceCollection.AddSingleton<VenueService>();        
            this._serviceProvider = serviceCollection.BuildServiceProvider();
            VenueDirectoryWindow = new VenueDirectoryWindow(this, httpClient);
            WindowDetail = new WindowDetail(this);

            // Register the window with the window system
            this.WindowSystem.AddWindow(VenueDirectoryWindow);
            this.WindowSystem.AddWindow(WindowDetail);

            pluginInterface.UiBuilder.Draw += DrawUI;
            pluginInterface.UiBuilder.OpenMainUi += ToggleVenueDirectoryWindow;
            commandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens the list of venues"
            });
        }

        private void ToggleVenueDirectoryWindow()
        {
            // Assuming you have an instance of your MainWindow accessible
            // For example, this could be stored in a property like this.PluginUi.MainWindow

            if (this.VenueDirectoryWindow.IsOpen)
            {
                // The window is currently open, so close it
                this.VenueDirectoryWindow.IsOpen = false;
                this.WindowDetail.IsOpen = false;
            }
            else
            {
                this.VenueDirectoryWindow.IsOpen = true;
            }
        }

        public void Dispose()
        {
            this._serviceProvider.Dispose();
            this.WindowSystem.RemoveAllWindows();

            VenueDirectoryWindow.Dispose();
            WindowDetail.Dispose();


            //TO DO: Dispose of the command
        }

        private void OnCommand(string command, string args)
        {

            VenueDirectoryWindow.IsOpen = true;

        }
        private Vector2 lastMainWinPos;
        private Vector2 lastMainWinSize;
        public void DrawUI()
        {
            this.WindowSystem.Draw();
        }




    }
}

