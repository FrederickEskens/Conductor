using System;
using System.IO;
using System.Xml.Serialization;

using ConductorAPI.Models.ApiAi;

namespace ConductorAPI.Business.IO
{
    public static class Connections
    {
        public static Models.iRail.Connections GetConnections(ApiAiModel model)
        {
            var from = model.Result.Parameters.From;
            var to = model.Result.Parameters.To;
            var time = model.Result.Parameters.Time;

            if (to == "Work")
            {
                to = Preferences.Preferences.GetUserPreference("Work");
            }

            if (string.IsNullOrEmpty(from))
            {
                from = Preferences.Preferences.GetUserPreference("Home");
            }

            var apiUrl = $"http://api.irail.be/connections/?to={to}&from={from}";

            if (!string.IsNullOrEmpty(time))
            {
                var ts = TimeSpan.FromHours(int.Parse(time));
                var formattedTime = ts.ToString("hhmm");

                apiUrl = $"http://api.irail.be/connections/?to={to}&from={from}&time={formattedTime}";

            }

            var xmlResponse = IoOperations.GetXmlDocumentFromUrl(apiUrl);

            Models.iRail.Connections result;

            using (TextReader sr = new StringReader(xmlResponse.OuterXml))
            {
                var serializer = new XmlSerializer(typeof(Models.iRail.Connections));
                result = (Models.iRail.Connections)serializer.Deserialize(sr);
            }

            return result;
        }
    }
}