using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

using ConductorAPI.Business.IO;
using ConductorAPI.Business.Preferences;
using ConductorAPI.Models.ApiAi;

namespace ConductorAPI.Controllers
{
    public class ConductorController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage WebHook(ApiAiModel model)
        {
            switch (model.Result.Action)
            {
                case "delay":
                    return Delay(model);
                case "info":
                    return Info(model);
                case "set.preference":
                    return SetPreference(model);
                default:
                    return IoOperations.GenerateJsonResponse("ConductorAPI could not be reached.");
            }
        }

        public HttpResponseMessage Delay(ApiAiModel model)
        {
            try
            {
                var connections = Connections.GetConnections(model);
                var connection = connections.ConnectionList[0];
                var nextConnection = connections.ConnectionList[0];

                string result;

                if (connection.Departure.IsCanceled)
                {
                    result =
                        $"The train from {connection.Departure.Station} to {connection.Arrival.Station} at {connection.Departure.Time.DateTime.TimeOfDay} has been canceled." +
                        $"The next train will be departing at {nextConnection.Departure.Time.DateTime:HH:mm}.";
                }
                else
                {
                    if (connection.Departure.Delay > 0)
                    {
                        result =
                            $"The train from {connection.Departure.Station} to {connection.Arrival.Station} has a delay of {connection.Departure.Delay / 60} minutes. " +
                            $"It will be departing at {connection.Departure.Time.DateTime.AddSeconds(connection.Departure.Delay):HH:mm}.";
                    }
                    else
                    {
                        result =
                            $"The train from {connection.Departure.Station} to {connection.Arrival.Station} has no delay. " +
                            $"It will be departing at {connection.Departure.Time.DateTime:HH:mm}.";
                    }
                }

                return IoOperations.GenerateJsonResponse(result);
            }
            catch (PreferenceException e)
            {
                return IoOperations.GenerateJsonResponse(e.Message);
            }
            catch (Exception)
            {
                return IoOperations.GenerateJsonResponse("I'm sorry, something went wrong.");
            }
        }

        public HttpResponseMessage Info(ApiAiModel model)
        {
            try
            {
                var connections = Connections.GetConnections(model);
                var connection = connections.ConnectionList.First(x => x.Departure.IsCanceled == false);

                var result =
                    $"The train from {connection.Departure.Station} to {connection.Arrival.Station} will be departing at {connection.Departure.Time.DateTime:HH:mm}.";

                return IoOperations.GenerateJsonResponse(result);
            }
            catch (PreferenceException e)
            {
                return IoOperations.GenerateJsonResponse(e.Message);
            }
            catch (Exception)
            {
                return IoOperations.GenerateJsonResponse("I'm sorry, something went wrong.");
            }

        }

        public HttpResponseMessage SetPreference(ApiAiModel model)
        {
            var preference = model.Result.Parameters.Preference;
            var value = model.Result.Parameters.Value;

            try
            {
                Preferences.SetUserPreference(preference, value);
                var result = $"Your preference {preference} has been set to {value}.";

                return IoOperations.GenerateJsonResponse(result);
            }
            catch (PreferenceException e)
            {
                return IoOperations.GenerateJsonResponse(e.Message);
            }
        }
    }
}