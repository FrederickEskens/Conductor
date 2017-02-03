using System;
using System.Xml.Serialization;

namespace ConductorAPI.Models.iRail
{
    [Serializable()]
    public class TravelInfo
    {
        [XmlAttribute("delay")]
        public int Delay { get; set; }

        [XmlAttribute("canceled")]
        public bool IsCanceled { get; set; }

        [XmlElement("station")]
        public string Station { get; set; }

        [XmlElement("time")]
        public Time Time { get; set; }
    }
}