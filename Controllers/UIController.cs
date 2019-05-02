using Swashbuckle.AspNetCore.SwaggerGen;
using LogoPaasSampleApp.DIServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAFCore.Common.Utils.Serialization;
using System;

namespace LogoPaasSampleApp.Controllers
{
    /// <summary>
    /// Class UIController.
    /// </summary>
    public class UIController : Controller
    {
        #region ..Backing Fields..

        /// <summary>
        /// The sample di service
        /// </summary>
        private readonly ISampleDIService _sampleDIService = null;

        #endregion

        #region ..Constructors..

        /// <summary>
        /// Initializes a new instance of the <see cref="UIController"/> class.
        /// </summary>
        /// <param name="sampleDIService">The sample di service.</param>
        public UIController(ISampleDIService sampleDIService) : base()
        {
            _sampleDIService = sampleDIService;            
        }

        #endregion        

        #region ..View..

        /// <summary>
        /// Secures the index.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Authorize]        
        public ActionResult SecureIndex()
        {
            string claimInfo = string.Join("<br/> ", User.Claims);
            ViewBag.UserInfo = claimInfo;

            return View();
        }

        /// <summary>
        /// Unsecures the index.
        /// </summary>
        /// <returns>ActionResult.</returns>        
        public ActionResult UnsecureIndex()
        {
            var a = NSerializer.JSONSimple.Serialize(DateTimeOffset.Now);
            var b = NSerializer.JSONSimple.Serialize(DateTime.Now);
            ViewBag.UserInfo = "";
            return View();
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>        
        public ActionResult Logout()
        {
            return new RedirectResult("/logout");
        }

        #endregion
    }
}
