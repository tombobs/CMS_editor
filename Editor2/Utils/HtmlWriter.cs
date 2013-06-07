using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Editor2.Models;

namespace Editor2.Utils
{
    public class HtmlWriter
    {

        public static void WriteHtml(string title, string type)
        {
            DocModel doc = SerialisationService.GetDoc(title, type);
            string html = MakeHtml(doc);
            StreamWriter writer = new StreamWriter(Constants.HTMLFolderPath + doc.Title + "-" + doc.Type + ".html");
            writer.Write(html);
            writer.Close();
        }

        public static string MakeHtml(DocModel doc)
        {
            string modal = @"<div id=""myModal"" class=""modal hide fade in"" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
                                <div class=""modal-header"">
                                    <button type=""button"" class=""close"" data-dismiss=""modal"" aria-hidden=""true"">×</button>
                                    <h3 id=""modal-title"">Editing - [PAGE_TITLE]</h3>
                                </div>
                                <div class=""modal-body"">
                                    <p id=""edit-element""></p>
                                </div>
                                <div class=""modal-footer"">
                                    <button id=""close-button"" class=""btn"" data-dismiss=""modal"" aria-hidden=""true"">Close</button>
                                    <button id=""save-button"" class=""btn btn-primary"">Save changes</button>
                                </div>
                            </div> <!-- /modal -->";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");

            sb.AppendLine("<head>");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"http://twitter.github.io/bootstrap/assets/css/docs.css\"></link>");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"http://twitter.github.io/bootstrap/assets/css/bootstrap-responsive.css\"></link>");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"http://twitter.github.io/bootstrap/assets/css/bootstrap.css\"></link>");
            sb.AppendLine("</head>");

            sb.AppendLine("<body>");
            sb.AppendLine("<div class=\"container\">");
            sb.AppendLine(modal);
            sb.AppendLine("<h2 id=\"page-title\">" + doc.Title + " - " + doc.Type + "</h2>");
            sb.AppendLine("<section id=\"gridSystem\">");
            foreach (Element el in doc.Elements)
            {
                if (el.SubElements == null || el.SubElements.Count == 0)
                {
                    sb.AppendLine("<div class=\"row-fluid show-grid\">");
                    sb.Append("<div class=\"element span12\" id=\"" + el.ElementId + "\">");
                    sb.Append(HandleElement(el));
                    sb.Append("</div>");
                    sb.AppendLine("</div> <!-- /row -->");
                }
                else
                {
                    sb.AppendLine("<div class=\"row-fluid show-grid\">");
                    sb.Append("<div class=\"multi-element span 12\" id=\"" + el.ElementId + "\">");
                    sb.AppendLine(HandleSubElements(el));
                    sb.AppendLine("</div>");
                    sb.AppendLine("</div> <!-- /row -->");
                }
            }
            sb.AppendLine("</section>");
            sb.AppendLine("</div> <!-- /container -->");
            sb.AppendLine("<script src=\"http://twitter.github.io/bootstrap/assets/js/jquery.js\"></script>");
            sb.AppendLine("<script type=\"text/javascript\" src=\"Editor.js\"></script>");
            sb.AppendLine("<script src=\"http://twitter.github.io/bootstrap/assets/js/bootstrap-modal.js\"></script>");
            sb.AppendLine("<script src=\"http://twitter.github.io/bootstrap/assets/js/holder/holder.js\"></script>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

        public static string HandleElement(Element el)
        {
            switch (el.Type)
            {
                case "Heading":
                    return "<h2>" + el.Content + "</h2>";
                case "SubHeading":
                    return "<h3>" + el.Content + "</h3>";
                case "Text":
                    return "<span>" + el.Content + "</span>/n";
                case "Code":
                    return "<code>" + el.Content + "</code>";                
                case "OL":
                    return HandleOL(el);
                case "UL":
                    return HandleUL(el);
                default:
                    return el.Content; // this will allow insertion of custom html
            }
        }

        public static string HandleOL(Element el)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ol>");
            string[] ListItems = el.Content.Split(',');
            foreach (string Item in ListItems)
            {
                sb.Append("<li>");
                sb.Append(Item);
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ol>");
            return sb.ToString();
        }
        
        public static string HandleUL(Element el)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ul>");
            string[] ListItems = el.Content.Split(',');
            foreach (string Item in ListItems)
            {
                sb.Append("<li>");
                sb.Append(Item);
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        public static string HandleSubElements(Element el)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Element subElement in el.SubElements)
            {
                sb.AppendLine(HandleElement(subElement));
            }
            return sb.ToString();
        }
    }

    //public static Dictionary<string, string> Mappings = new Dictionary<string, string>();
    ////this whole class is a massive hack.. should use a library here really.

    //public static void SetUpMappings()
    //{
    //    Mappings.Add("<b>", "<span class=\"bold\">");
    //    Mappings.Add("</b>", "</span>");
    //    Mappings.Add("<i>", "<span class=\"italic\">");
    //    Mappings.Add("</i>", "</span>");
    //    Mappings.Add("<u>", "<span class=\"underline\">");
    //    Mappings.Add("</u>", "</span>");
    //    //Mappings.Add
    //}
}