using LogoPaasSampleApp.Settings;
using NAFCore.Common.Types.Initialization;
using NAFCore.Common.Utils.Diagnostics.Logger;
using NAFCore.Common.Utils.Extensions;
using NAFCore.Common.Utils.Serialization;
using NAFCore.Platform.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogoPaasSampleApp.Utils
{
    public class MenuHelper
    {
        private const string MENUSERVICE_EUREKA_NAME = "NAFCORESERVICESMENU";
        private const string MENUSERVICE_BASE_API_PATH = "";

        public static void RegisterToMenuService(SampleAppSettings appSettings, MenuRegistrationRequest newRequest)
        {
            Inner_RegisterToMenuService(appSettings, newRequest).GetAwaiter().GetResult();
        }

        private static async Task Inner_RegisterToMenuService(SampleAppSettings appSettings, MenuRegistrationRequest newRequest)
        {
            var apiPath = MENUSERVICE_BASE_API_PATH + "/api/Menus/PostApplicationMenus?appId={0}";
            apiPath = string.Format(apiPath, NAFInitializationInfo.Current.AppSecurityID.ToString());
            //
            var client = GetMenuClient(appSettings);

            var content = new StringContent(NSerializer.JSONSimple.Serialize(new MenuRegistrationRequest[] { newRequest }), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            var result = await client.PostAsync(apiPath, content).ConfigureAwait(false);
            if (!result.IsSuccessStatusCode)
            {                
                var responseContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                NLogger.Instance().Error($"Menu registration error: {responseContent}");
            }
        }

        private static HttpClient GetMenuClient(SampleAppSettings appSettings)
        {
            string serviceUri = MENUSERVICE_EUREKA_NAME;
            if (appSettings.MenuServiceAddr.Assigned())
                serviceUri = appSettings.MenuServiceAddr;

            Uri requestUri = new Uri(((serviceUri.StartsWith("http://") || serviceUri.StartsWith("https://")) ? "" : "http://") + serviceUri);
            //
            var client = requestUri.NewClient();
            if (!client.DefaultRequestHeaders.Contains("client_id"))
                client.DefaultRequestHeaders.Add("client_id", NAFInitializationInfo.Current.AppSecurityID.ToString());

            if (!client.DefaultRequestHeaders.Contains("client_secret"))
                client.DefaultRequestHeaders.Add("client_secret", Startup.GetSecuritySecret().ToString());

            return client;
        }

        

        /*
         [
              {
                "AppId": "1076d753-4312-406e-9b9e-75cec8bc22c4",
                "Url": "http://localhost:5000/",
                "IsUrlRelative": false,
                "IsInNewTab": false,
                "LangResources": [
                  {
                    "Lang": "tr-TR",
                    "Name": "Bayi Sample App",
                   "Description": "Bayi Sample App",
                    "Tooltip": "Bayi Sample App",
                    "TenantId": "dd1f31ef-f6dc-40a5-89bb-a0ac4fbb3a54",
                    "MenuId": ""
                  }
                ],
                "ChartType": "Area",
                "TenantId": "dd1f31ef-f6dc-40a5-89bb-a0ac4fbb3a54",
                "Id": ""
              }
        ]

         
         */
    }
}
