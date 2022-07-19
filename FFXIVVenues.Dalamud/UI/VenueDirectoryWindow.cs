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
            ImGui.Text("A list of venues");
            ImGui.Spacing();
            if (ImGui.BeginTable("Table Id", 3))
            {
                ImGui.TableSetupColumn("Name");
                ImGui.TableSetupColumn("Value");
                ImGui.TableSetupColumn("Link");
                ImGui.TableHeadersRow();

                ImGui.TableNextColumn();
                ImGui.Text("Venue 1");
                ImGui.TableNextColumn();
                ImGui.Text("Jenova | Mist | Ward 1 | Plot 69");
                ImGui.TableNextColumn();
                ImGui.Text("https://ffxivvenues.com/#N0LXhs1w9K4z");

                ImGui.TableNextColumn();
                ImGui.Text("Venue 2");
                ImGui.TableNextColumn();
                ImGui.Text("Adamantoise | Lavender Beds | Ward 1 | Plot 69");
                ImGui.TableNextColumn();
                ImGui.Text("https://ffxivvenues.com/#cCWmQJJfT");

                ImGui.TableNextColumn();
                ImGui.Text("Venue 3");
                ImGui.TableNextColumn();
                ImGui.Text("Gilgamesh | Mist | Ward 1 | Plot 69");
                ImGui.TableNextColumn();
                ImGui.Text("https://ffxivvenues.com/#dXYvJTVDj");
            }
            ImGui.EndTable();
            ImGui.Spacing();
            ImGui.Text("Thank you for viewing this list of venues");
        }

    }
}
