using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace FFXIVVenues.Dalamud
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        public void Save(DalamudPluginInterface pluginInterface) =>
            pluginInterface!.SavePluginConfig(this);
    }
}
