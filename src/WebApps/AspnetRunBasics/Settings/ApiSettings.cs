using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetRunBasics.Settings
{
    public class ApiSettings : IApiSettings
    {
        public string BaseUrl { get; set; }
    }
}
