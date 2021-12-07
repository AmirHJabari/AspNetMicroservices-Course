using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetRunBasics.Settings
{
    public interface IApiSettings
    {
        string BaseUrl { get; set; }
    }
}
