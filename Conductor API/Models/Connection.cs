using System;
using System.Xml.Serialization;

namespace Conductor_API.Models
{
    [Serializable()]
    public class Connection
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("departure")]
        public TravelInfo Departure { get; set; }

        [XmlElement("arrival")]
        public TravelInfo Arrival { get; set; }

        [XmlElement("duration")]
        public int Duration { get; set; }
    }
}