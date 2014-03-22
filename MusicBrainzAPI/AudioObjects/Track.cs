using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MusicBrainzAPI.AudioObjects
{
    [XmlRoot("track", Namespace = "http://musicbrainz.org/ns/mmd-2.0#")]
    public class Track
    {
        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlElement("number")]
        public int Number { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("length")]
        public long Length { get; set; }
    }
}
