using System;
using System.Xml.Serialization;

namespace ConductorAPI.Models.iRail
{
    [Serializable()]
    public class Time
    {
        [XmlAttribute("formatted")]
        public DateTime DateTime { get; set; }
    }
}