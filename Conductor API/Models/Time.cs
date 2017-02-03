using System;
using System.Xml.Serialization;

namespace Conductor_API.Models
{
    [Serializable()]
    public class Time
    {
        [XmlAttribute("formatted")]
        public DateTime DateTime { get; set; }
    }
}