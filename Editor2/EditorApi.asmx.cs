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
    // [System.Web.Script.Services.ScriptService]
    public class EditorApi : System.Web.Services.WebService
    {

     //   public string FolderPath = 



       [WebMethod]
        public XmlDocument GetDocXml(string title)
        {
            System.IO.StreamReader read = new System.IO.StreamReader(Constants.FolderPath + title + ".xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(read.ReadToEnd());
            return xmlDoc;
        }

        [WebMethod]
        public string GetDocHtml(string title)
        {
            XmlDocument xml = GetDocXml(title);
            // do some XSLT/ASP magic here
            return ""; // return the html...        
        }

        [WebMethod]
        public void CreateNewDoc(string name, string type)
        {
            DocModel doc = SerialisationService.GetDocByTitleAndType(name , type);
            if (doc == null)
            {
                DocModel NewDoc = new DocModel(name, type);
                SerialisationService.SerialiseDoc(NewDoc);
            }
            else throw new Exception("Document : " + name + "-" + type + " already exists - not created.");
        }

        
        public void SaveUpdatedDoc(DocModel UpdatedDoc)
        {
            DocModel oldDoc = SerialisationService.GetDocByTitleAndType(UpdatedDoc.Title , UpdatedDoc.Type);
            
            // generate diff of docs here..

            //XmlDocument xmlDoc = GetDocXml(UpdatedDoc.Title);
            //StreamWriter write = new StreamWriter(FolderPath + UpdatedDoc.Title);
            SerialisationService.SerialiseDoc(UpdatedDoc);
        }

        //[WebMethod]
        //public void AddElementToDoc(

        [WebMethod]
        public void EditElementInDoc(string nameOfDoc, Guid element_id, string updatedContent)
        {
            //get the doc
            DocModel document = new DocModel();
            XmlSerializer x = new XmlSerializer(document.GetType());
            var obj = x.Deserialize(new StreamReader(Constants.FolderPath + nameOfDoc));
            document = (DocModel)obj;
            // get old element
            Element element = document.getElementByGuid(element_id);
            int pos = document.elements.IndexOf(element);
            document.elements.Remove(element);            
            element.Content = updatedContent;
            document.elements.Insert(pos, element);
            UpdateDoc(document);
        }


        public void WriteDoc (DocModel doc)
        {
            SerialisationService.SerialiseDoc(doc);
        }

        

        public void UpdateDoc(DocModel doc)
        {
            // assume we're sure we want to update it (checked valid name, elements etc...)
            SerialisationService.SerialiseDoc(doc);
        }

       

        [WebMethod]
        public string TestHtml()
        {
            DocModel doc = new DocModel();
            doc.StandardFormat = true;
            doc.Title = "doc2";
            doc.Type = "Overview";


            Element el1 = new Element();
            el1.Content = "content of el1";
            el1.DocId = 1;
            el1.Postion = 2;
            el1.Type = "Text";

            Element el2 = new Element();
            el2.Content = "content of el2";
            el2.DocId = 1;
            el2.Postion = 1;
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

            doc.elements.Add(el1);
            doc.elements.Add(el2);
            //doc.elements



            string result = HtmlWriter.GenerateHtml(doc);

            return result;
        }

    }
}

