using NAFCore.Common.Utils.Extensions;
using NAFCore.Common.Utils.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NAFCore.Platform.Services.Client;
using System.Threading.Tasks;
using System.Net.Http;
using LogoPaasSampleApp.Settings;

namespace LogoPaasSampleApp.Utils
{
    public class TenantHelper
    {
        private const string CLOUDCONTROLSERVICE_EUREKA_NAME = "CLOUDCONTROL";
        private static ConcurrentDictionary<Guid, object> _tenantAppSettings = new ConcurrentDictionary<Guid, object>();
        public static object GetTenantAppSettingsList(SampleAppSettings appSettings, Guid tenantId)
        {            
            if (tenantId != Guid.Empty && tenantId != DefaultValueExtensions.LOGO_TenantGuid())
            {
                object cachedSettings = null;
                if (!_tenantAppSettings.TryGetValue(tenantId, out cachedSettings))
                {

                    var result = Inner_GetTenantAppSettings(appSettings, tenantId).GetAwaiter().GetResult();

                    _tenantAppSettings.TryAdd(tenantId, result);
                    return result;
                }

                return cachedSettings;
            }
            return null;
        }

        public static Guid GetCurrentTenantId(SampleAppSettings appSettings)
        {
            return new Guid("dd1f31ef-f6dc-40a5-89bb-a0ac4fbb3a54");

            // Uncommet to fetch tenantid from 'ctxid' http header
            //return HttpContextExtensions.GetCurrentContextId();
        }

        private static async Task<JArray> Inner_GetTenantAppSettings(SampleAppSettings appSettings, Guid tenantId)
        {
            var apiPath = "/v1_0/NAF.Services.CloudControl.ApiWebService/api/tenants/{0}/applicationSegments/{1}/settings";
            apiPath = string.Format(apiPath, tenantId, "f0af89f4-c98a-4b95-815e-ae47c8a90d56");
            //
            var client = GetCloudControlClient(appSettings);
            var result = await client.GetAsync(apiPath).ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return NSerializer.JSONSimple.Deserialize<JArray>(responseContent);            
        }

        private static HttpClient GetCloudControlClient(SampleAppSettings appSettings)
        {
            string serviceUri = CLOUDCONTROLSERVICE_EUREKA_NAME;
            if (appSettings.CloudControlServiceAddr.Assigned())
                serviceUri = appSettings.CloudControlServiceAddr;

            Uri requestUri = new Uri(((serviceUri.StartsWith("http://") || serviceUri.StartsWith("https://")) ? "": "http://") + serviceUri);
            //
            var client = requestUri.NewClient();
            if (!client.DefaultRequestHeaders.Contains("client_id"))
                client.DefaultRequestHeaders.Add("client_id", "f0af89f4-c98a-4b95-815e-ae47c8a90d56");

            if (!client.DefaultRequestHeaders.Contains("client_secret"))
                client.DefaultRequestHeaders.Add("client_secret", "85a60f5e-ab96-4a0d-acdb-bab275872aaf");

            return client;
        }

    }
}
