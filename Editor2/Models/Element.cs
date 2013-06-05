using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Editor2.Models
{
    public class Element
    {
        public Element()
        {
            id = Guid.NewGuid();
        }

        [XmlAttribute]
        public Boolean IsSubElement;

        [XmlElement]
        public Guid id;

        [XmlElement]
        public string Content;

        [XmlElement]
        public List<Element> SubElements;

        [XmlAttribute]
        public int DocId;

        [XmlAttribute]
        public int Postion;

        [XmlAttribute]
        public string Type;//{ H1, H2, Text, OL, UL };

       // public enum Types { H1, H2, Text, OL, UL };

    }
}