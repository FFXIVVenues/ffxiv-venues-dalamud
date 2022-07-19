using Dalamud.Game.Gui;
using Dalamud.Interface;
using FFXIVVenues.Dalamud.Commands.Brokerage;
using FFXIVVenues.Dalamud.UI;
using System.Threading.Tasks;

namespace FFXIVVenues.Dalamud.Commands
{

    [Command("/showui", "Test command while we design a UI")]
    internal class ShowUiCommand : ICommandHandler
    {
        private readonly UiBuilder _uiBuilder;

        public ShowUiCommand(UiBuilder uiBuilder)
        {
            this._uiBuilder = uiBuilder;
        }

        public Task Handle(string args)
        {
            var newWindow = new VenueDirectoryWindow(this._uiBuilder);
            newWindow.Show();
            
            return Task.CompletedTask;
        }
    }

}
