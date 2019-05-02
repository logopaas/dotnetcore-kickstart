using LogoPaasSampleApp.DIServices.Interface;
using LogoPaasSampleApp.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogoPaasSampleApp.DIServices.Imp
{
    /// <summary>
    /// Class SampleDIService.
    /// </summary>
    public class SampleDIService : ISampleDIService
    {
        public SampleAppSettings Settings { get; private set; }

        public SampleDIService(SampleAppSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Messages the echo.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="userInfo">The user information.</param>
        /// <returns>System.String.</returns>
        public string MessageEcho(string msg, string userInfo)
        {            
            return $"{msg}, current user info: {userInfo}";
        }
    }
}
