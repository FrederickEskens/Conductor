using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Conductor_API.Models
{
    public class ApiAiModel
    {
        public Result Result { get; set; }
    }

    public class Result
    {
        public string Action { get; set; }
        public ParametersModel Parameters { get; set; }
    }

    public class ParametersModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Time { get; set; }
        public string Preference { get; set; }
        public string Value { get; set; }
    }
}