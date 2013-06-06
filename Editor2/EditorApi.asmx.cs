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
            DocModel doc = SerialisationService.GetDocByTitleAndType(title, type);
            return doc.Elements.Count;
        }

        [WebMethod]
        public void ElementCreate(string DocTitle, string DocType, string ElementContent, string ElementType, int pos)
        {
            pos--; // lets just call the first element '1' in the front end..
            DocModel doc = SerialisationService.GetDocByTitleAndType(DocTitle, DocType);
            
            if (doc.Elements.Count < pos)   // this should never happen but if it does 
            {                               // then we'll just stick the element at the bottom            
                pos = doc.Elements.Count;
            }
            Element element = new Element();
            element.Content = ElementContent;
            element.Type = ElementType;            
            doc.Elements.Insert(pos, element);            
            UpdateDoc(doc);
        }

        [WebMethod]
        public void DocDelete(string title, string type)
        {
            File.Delete(Constants.HTMLFolderPath + title + type + ".xml");
        }


       [WebMethod]
        public XmlDocument GetDocXml(string title)
        {
            System.IO.StreamReader read = new System.IO.StreamReader(Constants.XMLFolderPath + title + ".xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(read.ReadToEnd());
            return xmlDoc;
        }

        [WebMethod]
        public string GetDocHtml(string title, string type)
        {
            DocModel doc = SerialisationService.GetDocByTitleAndType(title, type);
            return HtmlWriter.GenerateHtml(doc);    
        }

        [WebMethod]
        public void DocCreate(string name, string type)
        {
            DocModel doc = SerialisationService.GetDocByTitleAndType(name , type);
            if (doc == null)
            {
                DocModel NewDoc = new DocModel(name, type);
                SerialisationService.SerialiseDoc(NewDoc);
            }
            else throw new Exception("Document : " + name + "-" + type + " already exists - not created.");
        }

        [WebMethod]
        public void ElementEdit(string title, string type, string element_id, string updatedContent)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            DocModel document = SerialisationService.GetDocByTitleAndType(title, type);
            Element element = document.getElementByGuid(Guid.Parse(element_id));
            int pos = document.Elements.IndexOf(element);
            document.Elements.Remove(element);            
            element.Content = updatedContent;
            document.Elements.Insert(pos, element);
            UpdateDoc(document);
            this.TestHtml(title, type);
            HttpContext.Current.Response.End();
        }

        [WebMethod]
        public void ElementDelete(string title, string type, string ElementId)
        {
            DocModel doc = SerialisationService.GetDocByTitleAndType(title, type);
            Element element = doc.getElementByGuid(Guid.Parse(ElementId));
            doc.Elements.Remove(element);
            UpdateDoc(doc);
        }

        [WebMethod]
        public void ElementMove(string title, string type, string ElementId, int newPos)
        {
            newPos--; // lets just call the first element '1' in the front end..
            DocModel doc = SerialisationService.GetDocByTitleAndType(title, type);
            if (newPos > doc.Elements.Count)
            {
                throw new Exception("Doc only has " + doc.Elements.Count + " elements - cannot move to position:" + newPos);
            }
            else
            {
                Element element = doc.getElementByGuid(Guid.Parse(ElementId));
                doc.Elements.Remove(element);
                doc.Elements.Insert(newPos, element);
                UpdateDoc(doc);
            }
        }

        public void UpdateDoc(DocModel doc)
        {
            // assume we're sure we want to update it (checked valid name, elements etc...)
            SerialisationService.SerialiseDoc(doc);
        }










        [WebMethod]
        public string TestHtml(string title, string type)
        {
            DocModel doc = SerialisationService.GetDocByTitleAndType(title, type);
            return HtmlWriter.GenerateHtml(doc);            
        }
       

        [WebMethod]
        public string TestHtml2()
        {
            DocModel doc = new DocModel();
            doc.StandardFormat = true;
            doc.Title = "doc2";
            doc.Type = "Overview";


            Element el1 = new Element();
            el1.Content = "content of el1";
            
           
            el1.Type = "Text";

            Element el2 = new Element();
            el2.Content = "content of el2";
            
            
            el2.Type = "Text";

            Element sub1 = new Element();
            sub1.Content = "sub1 content";
            sub1.Type = "UL";
            sub1.Content = "s1item1,s1item2,s1item3";


            Element sub2 = new Element();
            sub2.Content = "sub2 content";
            sub2.Type = "OL";
            sub2.Content = "s2item1,s2item2,s2item3";

            el1.SubElements = new List<Element>();

            el1.SubElements.Add(sub1);
            el1.SubElements.Add(sub2);

            doc.Elements.Add(el1);
            doc.Elements.Add(el2);
            //doc.elements



            string result = HtmlWriter.GenerateHtml(doc);

            return result;
        }

    }
}

