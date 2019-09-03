namespace LogoPaasSampleApp.Diagnostic
{
    using LogoPaasSampleApp.Settings;
    using NAFCore.Common.Types.Basic;
    using NAFCore.Diagnostics.Model;
    using NAFCore.Diagnostics.Model.Services;
    using NAFCore.Platform.Services.Client;
    using System.Collections.Generic;

    /// <summary>
    /// Cloud Control Diagnosis Service uygulması
    /// </summary>
    public class GenericDiagnosisService : DiagnosisService
    {
        private SampleAppSettings _setting;

        /// <summary>
        /// 
        /// </summary>
        public GenericDiagnosisService()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting">IDMSettings</param>
        internal GenericDiagnosisService(SampleAppSettings setting)
        {
            _setting = setting;
            Diagnoses = new List<Diagnosis>();

            List<NAFGuid> serviceKeyList = new List<NAFGuid>();
            List<NAFGuid> mustList = new List<NAFGuid>();

            mustList.Add(ServiceList.Values.IDM);
            mustList.Add(ServiceList.Values.CloudControl);
            mustList.Add(ServiceList.Values.Menu);

            serviceKeyList.Add(ServiceList.Values.Settings);
            serviceKeyList.Add(ServiceList.Values.IDM);
            serviceKeyList.Add(ServiceList.Values.Menu);
            serviceKeyList.Add(ServiceList.Values.CloudControl);

            Diagnoses.AddRange(new ServiceDependencyChecker(serviceKeyList, mustList).Diagnoses);
        }
    }
}
