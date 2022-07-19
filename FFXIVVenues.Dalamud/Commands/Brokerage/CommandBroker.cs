using Dalamud.Game.Command;
using FFXIVVenues.Dalamud.Utils;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace FFXIVVenues.Dalamud.Commands.Brokerage
{
    internal class CommandBroker : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandManager _commandManager;
        private TypeMap<ICommandHandler> _typeMap;

        public CommandBroker(IServiceProvider serviceProvider, CommandManager commandManager)
        {
            this._serviceProvider = serviceProvider;
            this._commandManager = commandManager;
            this._typeMap = new TypeMap<ICommandHandler>(serviceProvider);
        }

        public CommandBroker AddCommand<T>() where T : ICommandHandler
        {
            var @type = typeof(T);
            var attributes = @type.GetCustomAttributes<CommandAttribute>();

            if (!attributes.Any())
                throw new ArgumentException($"{@type.Name} does not have a Command attribute");

            foreach (var attribute in attributes)
            {
                _commandManager.AddHandler(attribute.CommandName, new CommandInfo(ExecuteHandler)
                {
                    HelpMessage = attribute.CommandDescription
                });
                _typeMap.Add<T>(attribute.CommandName);
            }

            return this;
        }

        public CommandBroker ScanForCommands(Assembly? assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            var handlerTypes = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ICommandHandler)));

            foreach (var type in handlerTypes)
            {
                var attributes = type.GetCustomAttributes<CommandAttribute>();
                foreach (var attribute in attributes)
                {
                    _commandManager.AddHandler(attribute.CommandName, new CommandInfo(ExecuteHandler)
                    {
                        HelpMessage = attribute.CommandDescription
                    });
                    _typeMap.Add(attribute.CommandName, type);
                }
            }

            return this;
        }

        private void ExecuteHandler(string command, string args)
        {
            if (!_typeMap.ContainsKey(command))
                return;

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var handler = _typeMap.Activate(command);
            handler?.Handle(args);
        }

        public void Dispose()
        {
            foreach (var key in this._typeMap.Keys)
                this._commandManager.RemoveHandler(key);
        }

    }
}
