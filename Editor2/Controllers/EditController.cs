using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Editor2.Models;
using Editor2.Utils;

namespace Editor2.Controllers
{
    public class EditController : Controller
    {
        public ActionResult LoadDocToEdit(string PageName)
        {
            DocModel doc = new DocModel();
            doc.StandardFormat = true;
            doc.Title = "doc_title";
            doc.Type = "Contents";


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
            sub1.Content = "sub2 content";
            sub1.Type = "OL";
            sub1.Content = "s2item1,s2item2,s2item3";
            
            el1.SubElements.Add(sub1);
            el1.SubElements.Add(sub2);

            doc.Elements.Add(el1);
            doc.Elements.Add(el2);
            //doc.elements


            HtmlWriter.GenerateHtml(doc);

            

            return View(doc);
        }
    }
}
