using Dalamud.Interface;
using FFXIVVenues.Dalamud.UI.Abstractions;
using ImGuiNET;
using System.Numerics;

namespace FFXIVVenues.Dalamud.UI
{
    internal class VenueDirectoryWindow : Window
    {
        public VenueDirectoryWindow(UiBuilder uiBuilder) : base(uiBuilder) {
            this.InitialSize = new Vector2(800, 100);
        }

        public override void Render()
        {
            // ImGui is it's own library that Dalamud is using
            // So, if you wanna know what interesting Gui elements
            // there are, look up "ImGui". This library is a port
            // from a popular C library, things may not look *exactly*
            // the same as here, but the premise the generally the same.

            ImGui.Text("Hello World");
        }

    }
}
