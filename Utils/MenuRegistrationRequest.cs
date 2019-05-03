using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogoPaasSampleApp.Utils
{
    public class MenuRegistrationRequest
    {
        public string AppId { get; set; }
        public string Url { get; set; }
        public bool IsUrlRelative { get; set; } = false;
        public bool IsInNewTab { get; set; } = false;

        public MenuRegistrationLangResource[] LangResources { get; set; }
        public string ChartType { get; set; } = "Area";
        public string TenantId { get; set; }
        public string Id { get; set; }
    }

    public class MenuRegistrationLangResource
    {
        public string Lang { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tooltip { get; set; }
        public string TenantId { get; set; }
        public string MenuId { get; set; }
    }
}
