using Dalamud.Interface.Windowing;
using FFXIVVenues.Dalamud.Data;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVVenues.Dalamud.UI
{
    public class WindowDetail : Window
    {
        private readonly Plugin _plugin;
        public VenueInfo Info;
        
        public WindowDetail(Plugin plugin) : base($"")
        {
            this.WindowName = $"Venue Info";
            _plugin = plugin;
            Info = new VenueInfo(); // Initialize Info here
        }

        public override void OnClose()
        {

        }

        public override void Draw()
        {
            if (Info == null) this.IsOpen = false;
            ImGui.Text($"Venue Name: {Info.Name}");
            ImGui.Text($"Venue Location: {Info.Location}");

            // Set the wrap position to the current window width (or any specific width you want)
            ImGui.PushTextWrapPos(ImGui.GetCursorPos().X + ImGui.GetContentRegionAvail().X);
            ImGui.Text($"Description: {Info.Description}");
            ImGui.PopTextWrapPos();
            ImGui.Text($"Is Open?: {Info.IsOpen}");
        }
    }
}
