using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Editor2.Utils;

namespace Editor2.Models
{
    public class DocModel
    {
        [XmlElement]
        public string Title;

        [XmlElement]
        public Guid DocId;

        [XmlElement]
        public string Type;

        [XmlElement]
        public List<Element> Elements = new List<Element>();

        [XmlAttribute]
        public Boolean StandardFormat;

        public DocModel() { }
        public DocModel(string title, string type)
        {
            // need to check that concat of these is unique before calling this!!!!!
            this.Title = title; // e.g. 'Amazon Integration'
            this.Type = type;   // e.g. Contents
            DocId = Guid.NewGuid();
        }

        public void InsertElement(Element element, int position) // offset = displacement from first element (0 = add as first element)
        {
            if (this.Elements.Count < position)
            {
                throw new Exception("Cannot insert at position : " + position + ", there are only " + this.Elements.Count + " elements in this doc.");
            }
            this.Elements.Insert(position, element);
        }

        public void AddElement(Element element)
        {
            this.Elements.Add(element);
        }

        public Element GetElementByPosition(int position)
        {
            if (this.Elements.Count < position)
            {
                return null;
            }
            return this.Elements[position];
        }

        public Element GetElementByGuid(Guid guid)
        {
            Element result = this.Elements.Find(
                delegate(Element el)
                {
                    return el.ElementId == guid;
                }
            );
            return result;
        }

        public Boolean ElementExists(Guid guid)
        {
            return (GetElementByGuid(guid) != null);
        }
    }
}