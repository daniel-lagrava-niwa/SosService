﻿using System;
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
        private string property;
        private string station;
        public string Station { get { return station; } set { station = value; } }
        public string Property { get { return property; } set { property = value; } }

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

            property = "QR"; // This is for Discharge, HG is for Height of Gauge
            station = "91401"; // ID of the station
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

            Status = ActivityStatus.Done;
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
