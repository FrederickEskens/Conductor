using System;
using System.IO;
using System.Web;

using Newtonsoft.Json.Linq;

namespace ConductorAPI.Business.Preferences
{
    public static class Preferences
    {
        public static string GetUserPreference(string key)
        {
            try
            {
                var json =
                    JObject.Parse(File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/preferences.json")));
                var result = json[key].Value<string>();
                if (string.IsNullOrEmpty(result))
                {
                    throw new PreferenceException($"Preference {key} is empty.");
                }
                return result;
            }
            catch (PreferenceException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PreferenceException($"Preference {key} was not found.", e);
            }
        }

        public static void SetUserPreference(string key, string value)
        {
            try
            {
                var json =
                    JObject.Parse(File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/preferences.json")));
                json[key] = value;
                File.WriteAllText(HttpContext.Current.Server.MapPath("~/App_Data/preferences.json"), json.ToString());
            }
            catch (Exception e)
            {
                throw new PreferenceException($"Preference {key} can not be set.", e);
            }
        }
    }
}