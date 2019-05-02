using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAFCore.Common.Localization.Extensions;
using NAFCore.Common.Utils.Extensions;
using NAFCore.Common.Utils.Serialization;
using System.IO;
using NAFCore.Platform.Services.Client;
using LogoPaasSampleApp.DIServices.Interface;
using NAFCore.Platform.Services.Hosting.Attributes;
using NAFCore.Platform.Services.Hosting.APIDoc.Attributes;
using Newtonsoft.Json.Linq;
using NAFCore.Common.Utils.Diagnostics.Logger;
using NAFCore.DAL.EF.Repositories;
using LogoPaasSampleApp.Dal.Entity;
using LogoPaasSampleApp.Utils;

namespace LogoPaasSampleApp.Controllers
{
    /// <summary>
    /// Class ApiController.
    /// </summary>
    [Route("api/[controller]")]
    [Produces(DefaultValueExtensions.MIMEType.AppJson)]
    public class ApiController : Controller
    {
        #region ..Backing Fields..

        private readonly ISampleDIService _sampleDIService = null;
        private readonly Repository<Customer, long> _customerRepo;

        #endregion

        #region ..Constructors..

        public ApiController(ISampleDIService sampleDIService, Repository<Customer, long> customerRepo) : base()
        {
            _sampleDIService = sampleDIService;
            _customerRepo = customerRepo;
        }

        #endregion

        #region ..Api..

        /// <summary>
        /// Adds a new customer to db
        /// </summary>
        /// <param name="c">Customer</param>
        /// <returns>Message Content</returns>
        [HttpPost("/api/addcustomer")]
        [SwaggerGroup("Secure Apis")]
       // [ClientAuthorize]
        public async Task<Customer> AddCustomer([FromBody]Customer c)
        {
            c = await _customerRepo.SaveOrUpdateAsync(c);
            return c;
        }

        /// <summary>
        /// Deletes customer
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>Message Content</returns>
        [HttpDelete("/api/deletecustomer")]
        [SwaggerGroup("Secure Apis")]
        // [ClientAuthorize]
        public bool DeleteCustomer(int customerId)
        {
            return _customerRepo.Delete(customerId);            
        }

        /// <summary>
        /// Retrieves customer info
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <returns>Message Content</returns>
        [HttpGet("/api/getcustomer")]
        [SwaggerGroup("Secure Apis")]
        // [ClientAuthorize]
        public Customer GetCustomer(int customerid)
        {
            return _customerRepo.FindById(customerid);            
        }

        /// <summary>
        /// Sample echo api for with token validation
        /// </summary>
        /// <param name="msg">Message Content</param>
        /// <returns>Message Content</returns>
        [HttpPost("/api/secureuserechoapi")]        
        [SwaggerGroup("Secure Apis")]
        [ClientAuthorize]
        public string SecureUserEchoApi(string msg)
        {
            string claimInfo = string.Join(", ", User.Claims);
            msg = "Hello from secure user api, the message is:" + msg;

            return _sampleDIService.MessageEcho(msg, claimInfo);
        }

        /// <summary>
        /// Sample echo api for with securityId/security secret validation
        /// </summary>
        /// <param name="msg">Message Content</param>
        /// <returns>Message Content</returns>
        [HttpPost("/api/secureappechoapi")]        
        [SwaggerGroup("Secure Apis")]
        [SecurityInfoValidation]
        public string SecureAppEchoApi(string msg)
        {            
            msg = "Hello from security info validated api, the message is:" + msg;

            return _sampleDIService.MessageEcho(msg, string.Empty);
        }

        /// <summary>
        /// Unsecure echo api
        /// </summary>
        /// <param name="msg">Message Content</param>
        /// <returns>Message Content</returns>
        [HttpGet("/api/echoapi")]
        [SwaggerOperation("LegacyUnsecureEchoApi")]
        [SwaggerGroup("Unsecure Apis")]
        public string UnsecureEchoApi(string msg)
        {            
            msg = "Hello from unsecure api, the message is:" + msg;

            return _sampleDIService.MessageEcho(msg, string.Empty);
        }

        #endregion
    }
}
