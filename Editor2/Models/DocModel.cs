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
        public List<Element> elements = new List<Element>();

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

        public void InsertElement(Element element, int pos) // offset = displacement from first element (0 = add as first element)
        {
            if (this.elements.Count < pos)
            {
                throw new Exception("Cannot insert at position : " + pos + ", there are only " + this.elements.Count + " elements in this doc.");
            }
            this.elements.Insert(pos, element);
        }

        public void AddElement(Element element)
        {
            this.elements.Add(element);
        }

        public Element getElementByPosition(int position)
        {
            if (this.elements.Count < position)
            {
                return null;
            }
            return this.elements[position];
        }

        public Element getElementByGuid(Guid guid)
        {
            Element result = this.elements.Find(
            delegate(Element bk)
            {
                return bk.id == guid;
            }
            );
            return result;
        }

        public Boolean ElementExists(Guid guid)
        {            
            Element result = this.elements.Find(
            delegate(Element bk)
            {
                return bk.id == guid;
            }
            );
            return (result != null);
        }
    }
}