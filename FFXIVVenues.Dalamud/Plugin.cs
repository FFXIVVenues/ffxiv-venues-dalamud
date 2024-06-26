﻿using System;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.Net.Http;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.DependencyInjection;
using FFXIVVenues.Dalamud.Commands.Brokerage;
using FFXIVVenues.Dalamud.UI.Abstractions;

namespace FFXIVVenues.Dalamud
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "FFXIV Venues";
        private readonly ServiceProvider _serviceProvider;

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager,
            [RequiredVersion("1.0")] IChatGui chatGui)
        {
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
            serviceCollection.AddSingleton<CommandBroker>();
            serviceCollection.AddSingleton<WindowBroker>();
            serviceCollection.AddSingleton<VenueService>();
                
            this._serviceProvider = serviceCollection.BuildServiceProvider();
            this._serviceProvider.GetService<CommandBroker>()?.ScanForCommands();
        }

        public void Dispose() =>
            this._serviceProvider.Dispose();

    }
}

