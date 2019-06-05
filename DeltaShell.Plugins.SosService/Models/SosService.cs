using System;
using DelftTools.Functions;
using DelftTools.Functions.Generic;
using DelftTools.Shell.Core.Workflow;
using DelftTools.Shell.Core.Workflow.DataItems;
using NetTopologySuite.Extensions.Coverages;
using GeoAPI.Extensions.Feature;

namespace DeltaShell.Plugins.SosService.Models
{
    public class SosService : ModelBase
    {
        private readonly SOSClientJSON.JSONClient jsonClient;
        private readonly FeatureCoverage timeSeries;
        public string StartTime { get { return StartTime; } set { StartTime = value; } }
        public string EndTime { get { return EndTime; } set { EndTime = value; } }
        public string Property { get { return Property; } set { Property = value; } }
        public string Station { get { return Station; } set { Station = value; } }

        public SosService()
        {
            jsonClient = new SOSClientJSON.JSONClient("http://wellsensorobsp.niwa.co.nz:8080/52n-sos-aquarius-webapp/service");
            StartTime = SOSClientJSON.Utils.TimeFormat.GetTimeFormatForQuery(2017, 3, 1);
            EndTime = SOSClientJSON.Utils.TimeFormat.GetTimeFormatForQuery(2017, 3, 2);
            timeSeries = new FeatureCoverage("Time Series")
            {
                IsTimeDependent = true,
                Arguments = { new Variable<IFeature>("Location") { FixedSize = 0 } },
                Components = { new Variable<double>("Value") },
            };

            Property = "QR"; // This is for Discharge, HG is for Height of Gauge
            // Station = "15341"; // ID of the station
            DataItems.Add(new DataItem(Station, "Station", typeof(string), DataItemRole.Input, "StationTag"));
            DataItems.Add(new DataItem(timeSeries, "Time Series", typeof(FeatureCoverage), DataItemRole.Output, "TimeSeriesTag"));
        }

        protected override void OnExecute()
        {
            var result = jsonClient.PerformTimeSeriesRequest(Property, Station, StartTime, EndTime);
        }

        protected override void OnInitialize()
        {
            Console.WriteLine("We are initializing this");
        }

        private void ValidateInputData()
        {
            Console.WriteLine("Validation of Input Data");
        }
    }
}
