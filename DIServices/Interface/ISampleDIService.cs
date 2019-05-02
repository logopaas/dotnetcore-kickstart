using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogoPaasSampleApp.DIServices.Interface
{
    /// <summary>
    /// Interface ISampleDIService
    /// </summary>
    public interface ISampleDIService
    {
        /// <summary>
        /// Messages the echo.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="userInfo">The user information.</param>
        /// <returns>System.String.</returns>
        string MessageEcho(string msg, string userInfo);
    }
}
