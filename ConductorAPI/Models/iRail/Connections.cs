using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConductorAPI.Models.iRail
{
    [Serializable()]
    [XmlRoot("connections")]
    public class Connections
    {
        [XmlElement("connection")]
        public List<Connection> ConnectionList { get; set; }
    }
}