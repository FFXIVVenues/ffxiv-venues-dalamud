using System.Threading.Tasks;

namespace FFXIVVenues.Dalamud.Commands.Brokerage
{
    internal interface ICommandHandler
    {
        Task Handle(string args);
    }
}
