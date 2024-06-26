﻿using Dalamud.Interface;
using FFXIVVenues.Dalamud.UI.Abstractions;
using FFXIVVenues.VenueModels;
using ImGuiNET;
using System.Numerics;
using System.Linq;
using System;
using Dalamud.Interface.Colors;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FFXIVClientStructs.FFXIV.Client.Graphics.Kernel;

namespace FFXIVVenues.Dalamud.UI
{
    internal class VenueDirectoryWindow : Window
    {
        private readonly VenueService _venueService;
        private readonly Task<Venue[]?> _venuesTask;

        public VenueDirectoryWindow(UiBuilder uiBuilder, HttpClient httpClient, VenueService venueService) : base(uiBuilder)
        {
            _venueService = venueService;
            this._venuesTask = httpClient.GetFromJsonAsync<Venue[]>("https://api.ffxivvenues.com/venue");
            this.InitialSize = new Vector2(800, 100);
            this.Title = "Open venues";
        }

        public override void Render()
        {
            // ImGui is it's own library that Dalamud is using
            ImGui.Text("A list of venues");
            ImGui.Spacing();

            if (!this._venuesTask.IsCompleted)
            {
                ImGui.Text("Loading...");
                return;
            }

            var venues = this._venuesTask.Result;
            
            //Fetch venues to show -- will be a method later
            if (ImGui.BeginTable("Table Id", 4))
            {
                ImGui.TableSetupColumn("Name");
                ImGui.TableSetupColumn("Address");
                ImGui.TableSetupColumn("Open until");
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
                        var color = ImGuiColors.DalamudWhite;
                        if (venue.IsOpen())
                            color = ImGuiColors.DalamudViolet;
                
                        //NAME COLUMN
                        ImGui.TableNextColumn();
                        ImGui.TextColored(color, venue.Name);
                
                        // LOCATION COLUMN
                        
                        ImGui.Image(this._venueService.GetVenueBanner(venue.Id).ImGuiHandle, new Vector2(100, 100));
                        ImGui.TableNextColumn();
                        ImGui.TextColored(color, venue.Location.ToString());
                        
                        // var opening = venue.Openings.FirstOrDefault(o => o.IsNow);
                        // var utcDate = DateTime.UtcNow;
                        // var closing = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, opening.End.Utc.Hour, opening.End.Utc.Minute, 0, DateTimeKind.Utc);
                        // if (closing < DateTime.UtcNow) closing = closing.AddDays(1);
                        // closing = closing.ToLocalTime();
                        // ImGui.TableNextColumn();
                        // ImGui.TextColored(color, closing.ToString("t"));
                
                        // //LINK COLUMN
                        // ImGui.TableNextColumn();
                        // if (ImGui.Button("Show " + venue.Name))
                        // {
                        //     OpenBrowser("https://ffxivvenues.com/#" + venue.Id);
                        // }
                        
                        //ImGui.TextColored(color, "https://ffxivvenues.com/#" + venue.Id);
                       
                
                    }
                }
            }
            ImGui.EndTable();
            ImGui.Spacing();
            ImGui.Text("Thank you for viewing this list of venues");
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }




    }
}
