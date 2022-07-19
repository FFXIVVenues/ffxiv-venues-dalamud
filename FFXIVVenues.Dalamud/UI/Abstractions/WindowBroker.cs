using System;
using Microsoft.Extensions.DependencyInjection;

namespace FFXIVVenues.Dalamud.UI.Abstractions;

internal class WindowBroker
{
    private readonly IServiceProvider _serviceProvider;

    public WindowBroker(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }
    
    public T? Create<T>() where T : Window
    {
        return ActivatorUtilities.CreateInstance(this._serviceProvider, typeof(T)) as T;
    }
}