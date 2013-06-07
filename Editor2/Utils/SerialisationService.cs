using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Editor2.Models;

namespace Editor2.Utils
{
    public class SerialisationService
    {
        public static void SerialiseDoc(DocModel doc)
        {
            // will overwrite existing xml
            System.IO.StreamWriter file = new System.IO.StreamWriter(Constants.XMLFolderPath + doc.Title + "-" + doc.Type + ".xml");
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(doc.GetType());
            x.Serialize(file, doc);
            file.Close();
        }

        public static DocModel GetDoc(string title, string type)
        {
            try
            {
                System.IO.StreamReader read = new System.IO.StreamReader(Constants.XMLFolderPath + title + "-" + type + ".xml");
                DocModel doc = new DocModel();
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(doc.GetType());
                var obj = x.Deserialize(read);
                read.Close();
                doc = (DocModel)obj;                
                return doc;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }
    }
}