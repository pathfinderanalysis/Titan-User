using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Titan.UFC.Users.WebAPI.Resources
{
    public class SharedResource : ISharedResource
    {
        private readonly IStringLocalizer _localizer;

        public SharedResource(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        LocalizedString ISharedResource.this[string name] => _localizer[name];
    }
}