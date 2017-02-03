using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

using Conductor_API.Models;

using Newtonsoft.Json;

namespace Conductor_API.Controllers
{
    public class ConductorController : ApiController
    {
        public const string Home = "Berlaar";
        public const string Work = "Gent-Dampoort";


        [HttpPost]
        public HttpResponseMessage WebHook(ApiAiModel model)
        {
            switch (model.Result.Action)
            {
                case "delay":
                    return Delay(model);
                case "info":
                    return Info(model);
                default:
                    return GenerateJsonResponse("Conductor API could not be reached.");
            }
        }

        public HttpResponseMessage Delay(ApiAiModel model)
        {
            var connections = GetConnections(model);

            var connection = connections.ConnectionList[0];
            var nextConnection = connections.ConnectionList[0];

            string result;

            if (connection.Departure.IsCanceled)
            {
                result = $"The train from {connection.Departure.Station} to {connection.Arrival.Station} at {connection.Departure.Time.DateTime.TimeOfDay} has been canceled." +
                         $"The next train will be departing at {nextConnection.Departure.Time.DateTime:HH:mm}.";
            }
            else
            {
                if (connection.Departure.Delay > 0)
                {
                    result = $"The train from {connection.Departure.Station} to {connection.Arrival.Station} has a delay of {connection.Departure.Delay / 60} minutes. " +
                             $"It will be departing at {connection.Departure.Time.DateTime:HH:mm}.";
                }
                else
                {
                    result = $"The train from {connection.Departure.Station} to {connection.Arrival.Station} has no delay. " +
                             $"It will be departing at {connection.Departure.Time.DateTime:HH:mm}.";
                }
            }

            return GenerateJsonResponse(result);

        }

        public HttpResponseMessage Info(ApiAiModel model)
        {
            var connections = GetConnections(model);
            var connection = connections.ConnectionList.First(x => x.Departure.IsCanceled == false);

            var result = $"The train from {connection.Departure.Station} to {connection.Arrival.Station} will be departing at {connection.Departure.Time.DateTime:HH:mm}.";

            return GenerateJsonResponse(result);
        }

        private Connections GetConnections(ApiAiModel model)
        {
            var from = model.Result.Parameters.From;
            var to = model.Result.Parameters.To;
            var time = model.Result.Parameters.Time;

            if (to == "Work")
            {
                to = Work;
            }

            if (string.IsNullOrEmpty(from))
            {
                from = Home;
            }

            var apiUrl = $"http://api.irail.be/connections/?to={to}&from={from}";

            if (!string.IsNullOrEmpty(time))
            {
                var ts = TimeSpan.FromHours(int.Parse(time));
                var formattedTime = ts.ToString("hhmm");

                apiUrl = $"http://api.irail.be/connections/?to={to}&from={from}&time={formattedTime}";

            }

            var xmlResponse = GetXmlDocumentFromUrl(apiUrl);

            Connections result;

            using (TextReader sr = new StringReader(xmlResponse.OuterXml))
            {
                var serializer = new XmlSerializer(typeof(Connections));
                result = (Connections)serializer.Deserialize(sr);
            }

            return result;
        }

        private XmlDocument GetXmlDocumentFromUrl(string url)
        {
            string xmlString;
            using (var wc = new WebClient())
            {
                xmlString = wc.DownloadString(url);
            }

            var result = new XmlDocument();
            result.LoadXml(xmlString);

            return result;
        }

        public HttpResponseMessage GenerateJsonResponse(string output)
        {
            var response = new Response
            {
                Speech = output,
                DisplayText = output,
            };

            var json = JsonConvert.SerializeObject(response);

            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
    }
}