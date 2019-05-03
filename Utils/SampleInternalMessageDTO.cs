using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogoPaasSampleApp.Utils
{
    /// <summary>
    /// Sample Dto for internal messaging
    /// </summary>
    public class SampleInternalMessageDTO : IInternalMessageDTO
    {
        /// <summary>
        /// CustomerId
        /// </summary>
        public string NewCustomerId { get; set; }
    }
}
