using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;

namespace Skanetrafiken.Crm
{
    public class AppInsightsLogger : IDisposable
    {
        private readonly TelemetryClient _client;

        public AppInsightsLogger()
        {
            TelemetryConfiguration configuration = TelemetryConfiguration.Active;
            _client = new TelemetryClient(configuration);
        }

        public void LogInformation(string message)
        {
            _client.TrackTrace(message, SeverityLevel.Information);
        }

        public void InfoFormat(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogInformation(message);
        }

        public void LogError(string message)
        {
            _client.TrackTrace(message, SeverityLevel.Error);
        }

        public void LogException(Exception exception, IDictionary<string, string> customproperties = null)
        {
            _client.TrackException(exception, customproperties);
        }

        public void SetGlobalProperty(string key, string value)
        {
            _client.Context.GlobalProperties[key] = value;
        }

        public void Dispose()
        {
            _client.Flush();
        }
    }
}