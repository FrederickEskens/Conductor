using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;

using ConductorAPI.Models;
using ConductorAPI.Models.ApiAi;
using ConductorAPI.Models.iRail;

using Newtonsoft.Json;

namespace ConductorAPI.Business.IO
{
    public static class IoOperations
    {
        public static XmlDocument GetXmlDocumentFromUrl(string url)
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

        public static HttpResponseMessage GenerateJsonResponse(string output)
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