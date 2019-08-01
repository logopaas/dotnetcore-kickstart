using NAFCore.Common.Attributes.Settings;
using NAFCore.DAL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NAFCore.Common.Utils.Extensions;

namespace LogoPaasSampleApp.Settings
{
    [NDefaultContextOnly]
    public class SampleAppSettings
    {
        #region ..Constructors..

        public SampleAppSettings()
        {
        }

        public SampleAppSettings(bool initialize)
        {
            if (initialize)
                Initialize();
        }

        #endregion

        #region ..Public..

        [NDisplayName("Sample Settings Item")]
        [NDescription("Sample Settings Item")]
        [NInput(isRequired: true)]
        [NDBConnectionEditor]
        public string SampleSettingsItem { get; set; }

        [NDisplayName("Sample App DB Connection Info")]
        [NDescription("Sample App DB Connection Info")]
        public SampleAppDbSettings DbSettings { get; set; } = new SampleAppDbSettings();

        [NDisplayName("Cloud Control Service Address")]
        [NDescription("Cloud Control Service Address")]
        public string CloudControlServiceAddr { get; set; } = "dev-linux.logo-paas.com:6900";

        [NDisplayName("Cloud Control Service Address")]
        [NDescription("Cloud Control Service Address")]
        public string MenuServiceAddr { get; set; } = "dev-linux.logo-paas.com:7000";

        [NDisplayName("Menu Registration Address")]
        [NDescription("Menu Registration Address")]
        public string MenuRegistrationUrl { get; set; } = "http://localhost:5000/";

        public void Initialize()
        {                        
            DbSettings.MigrateDatabase = true;            
        }
        #endregion

        #region ..IConfigValidation Members..

        public bool IsEmpty()
        {
            return (DbSettings.NotAssigned() || (DbSettings.Assigned()));
        }

        #endregion
    }
}
