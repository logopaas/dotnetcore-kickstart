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

        [NDisplayName("Cloud Control Service Addr")]
        [NDescription("Cloud Control Service Addr")]
        public string CloudControlServiceAddr { get; set; } = "dev-win.logo-paas.com:8282";

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
