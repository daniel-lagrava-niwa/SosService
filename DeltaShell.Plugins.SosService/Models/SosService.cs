using System;
using DelftTools.Functions;
using DelftTools.Functions.Generic;
using DelftTools.Shell.Core.Workflow;
using DelftTools.Shell.Core.Workflow.DataItems;
using NetTopologySuite.Extensions.Coverages;
using GeoAPI.Extensions.Feature;
using SOSClientJSON;
using SOSClientJSON.Utils;
using System.Collections.Generic;


namespace DeltaShell.Plugins.SosService.Models
{
    public class SosService : ModelBase
    {
        private readonly SOSClientJSON.JSONClient jsonClient;
        private readonly FeatureCoverage timeSeries;
        public readonly string StartTime;
        public readonly string EndTime;
        public readonly string Property;
        public readonly string Station;

        public SosService()
        {
            jsonClient = new JSONClient("http://wellsensorobsp.niwa.co.nz:8080/52n-sos-aquarius-webapp/service");
            StartTime = TimeFormat.GetTimeFormatForQuery(2017, 3, 1);
            EndTime = TimeFormat.GetTimeFormatForQuery(2017, 3, 2);
            timeSeries = new FeatureCoverage("Time Series")
            {
                IsTimeDependent = true,
                Arguments = { new Variable<IFeature>("Location") { FixedSize = 0 } },
                Components = { new Variable<double>("Value") },
            };

            Property = "QR"; // This is for Discharge, HG is for Height of Gauge
            Station = "15341"; // ID of the station
            DataItems.Add(new DataItem(Station, "Station", typeof(string), DataItemRole.Input, "StationTag"));
            DataItems.Add(new DataItem(timeSeries, "Time Series", typeof(FeatureCoverage), DataItemRole.Output, "TimeSeriesTag"));
        }

        protected override void OnExecute()
        {
            TimeSeriesObject result = jsonClient.PerformTimeSeriesRequest(Property, Station, StartTime, EndTime);
            // TODO: add the parsing of the (lat,lon,timeSeries) object to the Coverage
            TimeSeries outputSeries = new TimeSeries { Components = { new Variable<double>(Property) } };
            outputSeries.Name = Station;
            Dictionary<string, decimal> inputSeries = result.TimeSeries;
            foreach(var item in inputSeries)
            {
                var time = DateTime.Parse(item.Key);
                outputSeries[time] = decimal.ToDouble(item.Value);
            }

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
