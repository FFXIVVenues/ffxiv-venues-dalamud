using Dalamud.Interface;
using FFXIVVenues.Dalamud.Commands.Brokerage;
using FFXIVVenues.Dalamud.UI;
using FFXIVVenues.VenueModels;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using FFXIVVenues.Dalamud.UI.Abstractions;

namespace FFXIVVenues.Dalamud.Commands
{

    [Command("/venues", "Show all venues")]
    internal class ShowUiCommand : ICommandHandler
    {
        private readonly WindowBroker _windowBroker;

        public ShowUiCommand(WindowBroker windowBroker)
        {
            this._windowBroker = windowBroker;
        }

        public Task Handle(string args)
        {
            var newWindow = this._windowBroker.Create<VenueDirectoryWindow>();
            newWindow?.Show();
            return Task.CompletedTask;
        }
    }

}
