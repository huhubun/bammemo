using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Abstractions.WebApiModels.SiteLinks
{
    public class UpdateSiteLinkRequest
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
    }
}
