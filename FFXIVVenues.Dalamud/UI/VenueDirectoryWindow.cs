using Dalamud.Interface;
using FFXIVVenues.Dalamud.UI.Abstractions;
using FFXIVVenues.VenueModels.V2022;
using ImGuiNET;
using System.Numerics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System;

namespace FFXIVVenues.Dalamud.UI
{
    internal class VenueDirectoryWindow : Window
    {
        Venue[] venues;
        public VenueDirectoryWindow(UiBuilder uiBuilder, Venue[] venues) : base(uiBuilder) {
            this.InitialSize = new Vector2(800, 100);
            this.venues = venues;
        }

        public override void Render()
        {
            // ImGui is it's own library that Dalamud is using
            ImGui.Text("A list of venues");
            ImGui.Spacing();

        //Fetch venues to show -- will be a method later
       

            if (ImGui.BeginTable("Table Id", 3))
            {
                ImGui.TableSetupColumn("Name");
                //ImGui.TableSetupColumn("Address");
                ImGui.TableSetupColumn("Link");
                ImGui.TableHeadersRow();

                if (venues == null)
                {
                    //Message = "Could not fetch venues at this time.",
                    ImGui.CloseCurrentPopup(); //show message later
                }
                else
                {
                    foreach(var venue in venues)
                    {
                        var opening = venue.Openings.FirstOrDefault(o => o.IsNow);
                        if (opening == null)
                        {
                            //NAME TAB
                            ImGui.TableNextColumn();
                            ImGui.Text(venue.Name);
                            //ImGui.TableNextColumn();
                            //ImGui.Text("TODO - Time"); // TIME OF OPENING -- hh:mm - hh:mm
                            ImGui.TableNextColumn();
                            
                            //ADDRESS TAB
                            //check if apt and is sub
                            if (venue.Location.Apartment != 0 && venue.Location.Subdivision == true)
                            {
                                ImGui.Text(
                                    venue.Location.DataCenter + "|" +
                                    venue.Location.World + "|" +
                                    venue.Location.District + "|" +
                                    venue.Location.Ward.ToString() + "|" +
                                    "Sub|Apt " +
                                    venue.Location.Apartment.ToString()
                                    );
                            }
                            //check if apt and not sub
                            else if (venue.Location.Apartment != 0 && venue.Location.Subdivision == false)
                            {
                                ImGui.Text(
                                    venue.Location.DataCenter + "|" +
                                    venue.Location.World + "|" +
                                    venue.Location.District + "|W " +
                                    venue.Location.Ward.ToString() + "|" +
                                    "Apt " +
                                    venue.Location.Apartment.ToString()
                                    );
                            }
                            //check if fc room
                            else if (venue.Location.Room != 0 && venue.Location.Subdivision == false)
                            {
                                ImGui.Text(
                                    venue.Location.DataCenter + "|" +
                                    venue.Location.World + "|" +
                                    venue.Location.District + "|W " +
                                    venue.Location.Ward.ToString() + "| P " +
                                    venue.Location.Plot.ToString() + "| Room " +
                                    venue.Location.Room.ToString()
                                    );
                            }
                            //else is *I hope* a plot :3
                            else
                            {
                                ImGui.Text(
                                    venue.Location.DataCenter + "|" +
                                    venue.Location.World + "|" +
                                    venue.Location.District + "|W " +
                                    venue.Location.Ward.ToString() + "| P " +
                                    venue.Location.Plot.ToString()
                                    );
                            }

                            //LINK TAB
                            ImGui.TableNextColumn();
                            ImGui.Text("https://ffxivvenues.com/#" + venue.Id);
                        }
                       

                    }
                }
            }
            ImGui.EndTable();
            ImGui.Spacing();
            ImGui.Text("Thank you for viewing this list of venues");
        }

    }
}
