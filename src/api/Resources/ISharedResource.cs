using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Titan.UFC.Users.WebAPI.Resources
{
    public interface ISharedResource
    {
        LocalizedString this[string name] { get; }
    }
}
