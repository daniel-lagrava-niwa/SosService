using System;
using DelftTools.Shell.Core.Workflow;
using log4net;


namespace DeltaShell.Plugins.SosService.Models
{
    class SosServicePreQuery : ModelBase
    {
        private readonly SOSClientJSON.JSONClient jsonClient;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SosServicePreQuery));     // handle for writing log messages

        public SosServicePreQuery()
        {
            jsonClient = new SOSClientJSON.JSONClient("http://wellsensorobsp.niwa.co.nz:8080/52n-sos-aquarius-webapp/service");
        }

        protected override void OnExecute()
        {
            SOSClientJSON.Utils.StationsData stationData = jsonClient.PerformCapabilitiesRequest();
            Log.Debug(stationData.ToString());

            Status = ActivityStatus.Done;
        }

        protected override void OnInitialize()
        {
            Console.WriteLine("Initialization step");
        }
    }
}
