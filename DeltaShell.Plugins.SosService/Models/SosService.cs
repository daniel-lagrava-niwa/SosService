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
        private string property;
        private string station;
        public string StartDate;
        public string EndDate;

        public string Station { get { return station; } set { station = value; } }
        public string Property { get { return property; } set { property = value; } }
        
        public SosService()
        {
            jsonClient = new JSONClient("http://wellsensorobsp.niwa.co.nz:8080/52n-sos-aquarius-webapp/service");
            StartDate = "2017-03-01";
            EndDate = "2017-03-02";
            
            timeSeries = new FeatureCoverage("Time Series")
            {
                IsTimeDependent = true,
                Arguments = { new Variable<IFeature>("Location") { FixedSize = 0 } },
                Components = { new Variable<double>("Value") },
            };

            property = "QR"; // This is for Discharge, HG is for Height of Gauge
            station = "91401"; // ID of the station

            AddDataItemSet<TimeSeries>(new List<TimeSeries>(), "Results", DataItemRole.Output, "ResultsTag", false);
            // DataItems.Add(new DataItem(timeSeries, "Time Series", typeof(FeatureCoverage), DataItemRole.Output, "TimeSeriesTag"));
        }

        protected override void OnExecute()
        {
            var StartTime = CreateQueryDate(StartDate);
            var EndTime = CreateQueryDate(EndDate);
            TimeSeriesObject result = jsonClient.PerformTimeSeriesRequest(Property, Station, StartTime, EndTime);
            // TODO: add the parsing of the (lat,lon,timeSeries) object to the Coverage
            TimeSeries outputSeries = new TimeSeries { Components = { new Variable<double>(Property) } };
            outputSeries.Name = Property+ "-" + Station;
            Dictionary<string, decimal> inputSeries = result.TimeSeries;
            foreach(var item in inputSeries)
            {
                var time = DateTime.Parse(item.Key);
                outputSeries[time] = decimal.ToDouble(item.Value);
            }
            var resultItems = this.GetDataItemSetByTag("ResultsTag").AsEventedList<TimeSeries>();
            resultItems.Add(outputSeries);
            
            Status = ActivityStatus.Done;
        }

        public string CreateQueryDate(string date)
        {
            DateTime parsedDate = DateTime.Parse(date);
            var year = parsedDate.Year;
            var month = parsedDate.Month;
            var day = parsedDate.Day;
            return TimeFormat.GetTimeFormatForQuery(year, month, day);
        }

        protected override void OnInitialize()
        {
            Console.WriteLine("We are initializing this");
            timeSeries.Clear();
        }

        private void ValidateInputData()
        {
            Console.WriteLine("Validation of Input Data");
        }
    }
}
