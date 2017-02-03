using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Conductor_API.Models
{
    [Serializable()]
    [XmlRoot("connections")]
    public class Connections
    {
        [XmlElement("connection")]
        public List<Connection> ConnectionList { get; set; }
    }
}