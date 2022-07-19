using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FFXIVVenues.Dalamud.Utils
{
    internal class TypeMap<T> where T : class
    {

        public string[] Keys => this._typeMap.Keys.ToArray();

        private readonly Dictionary<string, Type> _typeMap = new();
        private readonly IServiceProvider _serviceProvider;

        public TypeMap(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TypeMap<T> Add<A>(string key) where A : T
        {
            _typeMap.Add(key, typeof(A));
            return this;
        }

        public TypeMap<T> Add(string key, Type type)
        {
            var @requiredType = typeof(T);
            if (!type.IsAssignableTo(@requiredType))
                throw new ArgumentException($"Type {type.Name} is not of type {@requiredType.Name}");
            _typeMap.Add(key, type);
            return this;
        }

        public bool ContainsKey(string key)
        {
            return _typeMap.ContainsKey(key);
        }

        public Type Get(string key)
        {
            return _typeMap[key];
        }

        public T? Activate(string key, IServiceProvider? serviceProvider = null)
        {
            if (serviceProvider == null)
                serviceProvider = this._serviceProvider;

            var hasKey = _typeMap.ContainsKey(key);
            if (!hasKey)
            {
                return default;
            }
            return ActivatorUtilities.CreateInstance(serviceProvider, _typeMap[key]) as T;
        }

    }
}
