using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Editor2.Models;
using Editor2.Utils;
using System.Text;

namespace Editor2
{
    /// <summary>
    /// Summary description for EditorApi
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class EditorApi : System.Web.Services.WebService
    {
        [WebMethod]
        public int GetNumElementsInDoc(string title, string type)
        {
            DocModel doc = SerialisationService.GetDoc(title, type);
            return doc.Elements.Count;
        }

        [WebMethod]
        public void ElementCreate(string DocTitle, string DocType, string ElementContent, string ElementType, int position)
        {
            position--; // let's start counting at 1 in the front end..
            DocModel doc = SerialisationService.GetDoc(DocTitle, DocType);
            
            if (doc.Elements.Count < position)   // this should never happen but if it does 
            {                               // then we'll just stick the element at the bottom            
                position = doc.Elements.Count;
            }
            Element element = new Element();
            element.Content = ElementContent;
            element.Type = ElementType;            
            doc.Elements.Insert(position, element);            
            UpdateDoc(doc);
        }

        [WebMethod]
        public void DocDelete(string title, string type)
        {
            File.Delete(Constants.HTMLFolderPath + title + "-" + type + ".html");
            File.Delete(Constants.XMLFolderPath + title + "-" + type + ".xml");
        }


        [WebMethod]
        public XmlDocument GetDocXml(string title)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(Constants.XMLFolderPath + title + ".xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(file.ReadToEnd());
            return xmlDoc;
        }

        [WebMethod]
        public string GetDocHtml(string title, string type)
        {
            DocModel doc = SerialisationService.GetDoc(title, type);
            return HtmlWriter.MakeHtml(doc);    
        }

        [WebMethod]
        public void DocCreate(string name, string type)
        {
            DocModel doc = SerialisationService.GetDoc(name , type);
            if (doc == null)
            {
                DocModel NewDoc = new DocModel(name, type);
                SerialisationService.SerialiseDoc(NewDoc);
            }
            else throw new Exception("Document : " + name + "-" + type + " already exists - not created.");
        }

        [WebMethod]
        public void CreateHtml(string title, string type) 
        {
            HtmlWriter.WriteHtml(title, type);
        }

        // this one handles list elements
        [WebMethod]
        public void EditElement(string title, string type, string ElementId,
            List<string> updatedItems) 
        
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            DocModel doc = SerialisationService.GetDoc(title, type);
            Element element = doc.GetElementByGuid(Guid.Parse(ElementId));

            StringBuilder sb = new StringBuilder();
            for (int i = 0 ; i < updatedItems.Count - 1 ; i++)
            {
                sb.Append(updatedItems[i]);
            }
            sb.Append(updatedItems[updatedItems.Count - 1]);
            
            int pos = doc.Elements.IndexOf(element);
            doc.Elements.Remove(element);
            element.Content = sb.ToString();
            doc.Elements.Insert(pos, element);
            UpdateDoc(doc);
            HttpContext.Current.Response.End();
        }

        [WebMethod]
        public void ElementEdit(string title, string type, string elementId, string updatedContent)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            DocModel document = SerialisationService.GetDoc(title, type);
            Element element = document.GetElementByGuid(Guid.Parse(elementId));
            int pos = document.Elements.IndexOf(element);
            document.Elements.Remove(element);            
            element.Content = updatedContent;
            document.Elements.Insert(pos, element);
            UpdateDoc(document);            
            HttpContext.Current.Response.End();
        }

        [WebMethod]
        public void ElementDelete(string title, string type, string ElementId)
        {
            DocModel doc = SerialisationService.GetDoc(title, type);
            Element element = doc.GetElementByGuid(Guid.Parse(ElementId));
            doc.Elements.Remove(element);
            UpdateDoc(doc);
        }

        [WebMethod]
        public void ElementMove(string title, string type, string ElementId, int newPos)
        {
            newPos--; // lets just call the first element '1' in the front end..
            DocModel doc = SerialisationService.GetDoc(title, type);
            if (newPos > doc.Elements.Count)
            {
                throw new Exception("Doc only has " + doc.Elements.Count + " elements - cannot move to position:" + newPos);
            }
            else
            {
                Element element = doc.GetElementByGuid(Guid.Parse(ElementId));
                doc.Elements.Remove(element);
                doc.Elements.Insert(newPos, element);
                UpdateDoc(doc);
            }
        }

        public void UpdateDoc(DocModel doc)
        {            
            SerialisationService.SerialiseDoc(doc);
            HtmlWriter.MakeHtml(doc);
        }
    }
}

