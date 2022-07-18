using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using FFXIVVenues.Dalamud.Commands.Brokerage;
using FFXIVVenues.VenueModels.V2022;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FFXIVVenues.Dalamud.Commands
{

    [Command("/venues", "Get all open venues")]
    internal class GetVenuesCommand : ICommandHandler
    {
        private readonly HttpClient _httpClient;
        private readonly ChatGui _chatGui;

        public GetVenuesCommand(HttpClient httpClient, ChatGui chatGui)
        {
            this._httpClient = httpClient;
            this._chatGui = chatGui;
        }

        public async Task Handle(string args)
        {
            this._chatGui.Print("Getting venues...");

            var venues = await this._httpClient.GetFromJsonAsync<Venue[]>("https://api.ffxivvenues.com/venue?open=true");
            if (venues == null)
            {
                this._chatGui.PrintChat(new()
                {
                    Message = "Could not fetch venues at this time.",
                    Type = XivChatType.ErrorMessage
                });
                return;
            }

            foreach (var venue in venues)
            {
                var opening = venue.Openings.FirstOrDefault(o => o.IsNow);
                if (opening == null)
                {
                    this._chatGui.PrintChat(new()
                    {
                        Message = $"{venue.Name} is open.",
                        Type = XivChatType.Notice
                    });
                    continue;
                }
                var utcDate = DateTime.UtcNow;
                var closing = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, opening.End.Utc.Hour, opening.End.Utc.Minute, 0, DateTimeKind.Utc);
                if (closing < DateTime.UtcNow) closing = closing.AddDays(1);
                closing = closing.ToLocalTime();
                this._chatGui.PrintChat(new()
                {
                    Message = $"{venue.Name} is open until {closing:t}.",
                    Type = XivChatType.Notice
                });
            }

        }
    }

}
