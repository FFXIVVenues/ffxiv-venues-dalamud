using System;

namespace FFXIVVenues.Dalamud.Commands.Brokerage
{
    internal class CommandAttribute : Attribute
    {

        public string CommandName { get; set; }
        public string CommandDescription { get; set; }

        public CommandAttribute(string commandName, string commandDescription = null) 
        { 
            this.CommandName = commandName; 
            this.CommandDescription = commandDescription; 
        }

    }
}
